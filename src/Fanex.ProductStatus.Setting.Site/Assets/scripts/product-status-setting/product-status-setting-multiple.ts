class ProductStatusSettingMultipe extends ProductStatusSettingBase {
    $: JQueryStatic;
    $toggleAllArray: HTMLElement[];
    $toggleArray: HTMLElement[];
    $dialogConfirm: any;

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow);
    }

    init() {
        super.init();
        this.registerEvents();
        this.initDialog();
    }

    loadMultiple = () =>
        this.$("#productIds").change(() => {
            this.$("#productId").val(this.$("#productIds").val());
            this.$("#product-form").submit();
        });

    cachedElements() {
        super.cachedElements();

        this.$toggleAllArray = this.$thead.find("input[class='checkbox-hide']").toArray() as HTMLElement[];
        this.$toggleArray = this.$tbody.find("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enableAll').val();
        this.$resources.disabled = this.$('#disableAll').val();
        this.$resources.updateActionName = "MultipleUpdate";
    }

    bindingToggleEvent = () =>
        this.$toggleArray.forEach((element) =>
            element.addEventListener('click', () => this.toogleStatus(element)));

    bindingToggleAllEvent = () =>
        this.$toggleAllArray.forEach((element) =>
            element.addEventListener('click', () => this.toogleStatusAll(element)));

    registerEvents() {
        this.loadMultiple();

        this.bindingToggleAllEvent();
        this.bindingToggleEvent();
        this.$("#btnReset").on("click", () => this.$("#product-form").submit());
        this.$("#btnSubmit").on("click", (e: any) => {
            e.preventDefault();
            const updateViewModel = this.getUpdateViewModel();

            updateViewModel.productStatus.length > 0 ?
                this.$dialogConfirm.dialog("open") :
                this.showMessage(this.$errorCodeDefault, this.$resources.thereIsNoChange);
        });
    }

    initDialog(): void {
        const self: any = this;
        const dialogButtons: any = [
            {
                text: "Yes",
                "class": "btn -submit",
                click: () => {
                    this.$dialogConfirm.dialog("close");

                    const updateViewModel = self.getUpdateViewModel();

                    self.$.blockUI({
                        blockMsgClass: "block-ui-loading"
                    });

                    updateViewModel.productStatus.length > 0 ?
                        self.callAjax(updateViewModel, self.$resources.updateActionName) :
                        self.showMessage(self.$errorCodeDefault, self.$resources.thereIsNoChange);
                }
            },
            {
                text: "No",
                "class": "btn -normal",
                click: () => {
                    this.$dialogConfirm.dialog("close");
                }
            }
        ];

        this.$dialogConfirm = this.$("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: "auto",
            minHeight: 68,
            width: 380,
            modal: true,
            buttons: dialogButtons
        });
    }

    toogleStatusAll(toogleAll: HTMLElement) {
        if (this.allowActionOnElement(toogleAll)) {
            this.toggleStatus(toogleAll);

            let roleId = this.getRoleId(toogleAll);
            let enable = this.isChecked(toogleAll);

            let toogles = this.getToggles(this.$toggleArray, roleId);
            toogles.forEach((toogle) => {
                if (this.allowActionOnElement(toogle) && this.isChecked(toogle) != enable) {
                    toogle.click();
                }
            });
        }
    }

    toogleStatus(toogle: HTMLElement) {
        if (this.allowActionOnElement(toogle)) {
            this.toggleStatus(toogle);
            let roleId = this.getRoleId(toogle);

            this.watchHeader(roleId, false);

            if (roleId > 1) {
                let sportType = this.getSportType(toogle);
                let bettype = this.getBetType(toogle);
                let enable = this.isChecked(toogle);
                let downlineToggle = this.getToggleElements(this.$toggleArray, roleId - 1, sportType, bettype);

                this.toogleStatusDownline(downlineToggle, enable);
            }
        }
    }

    toogleStatusDownline(toogle: HTMLElement, enabledUpline: boolean) {
        this.toggleStatusByValue(toogle, enabledUpline);
        enabledUpline ? this.enableElement(toogle) : this.disableElement(toogle);

        let roleId = this.getRoleId(toogle);
        this.watchHeader(roleId, true);

        if (roleId > 1) {
            let sportType = this.getSportType(toogle);
            let bettype = this.getBetType(toogle);
            let enable = this.isChecked(toogle);
            let downlineToggle = this.getToggleElements(this.$toggleArray, roleId - 1, sportType, bettype);

            this.toogleStatusDownline(downlineToggle, enable);
        }
    }

    getToggleElements(toggleArray: HTMLElement[], roleId: number, sportType: number, bettype: number): HTMLElement {
        for (let index = 0; index < toggleArray.length; index++) {
            let toggle = toggleArray[index];
            if (this.getRoleId(toggle) == roleId && this.getBetType(toggle) == bettype && this.getSportType(toggle) == sportType) {
                return toggle;
            }
        }

        return null;
    }

    toggleStatus = (toggle: HTMLElement) => this.toggleStatusByValue(toggle, this.isChecked(toggle));

    toggleStatusByValue = (toggle: HTMLElement, enable: boolean) => {
        this.$(toggle).prop('checked', enable);
        let labelElement = this.getNextLabel(toggle);

        let tooltip = this.getTooltip(labelElement, enable);
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;
        let labelClass = enable ? this.$resources.enabledClass : this.$resources.disabledClass;
        let bettype = this.getBetType(toggle);

        this.$(labelElement).attr("title", tooltip)
            .removeAttr('class')
            .attr('class', labelClass)
            .text(labelText);
    }

    watchHeader = (roleId: number, isDownline: boolean) => {
        if (this.$isMultipleBetTypes) {
            [true, false].forEach((status) => {
                this.toogleHeader(roleId, status, isDownline);
            });
        }
    }

    toogleHeader = (roleId: number, status: boolean, isDownline: boolean) => {
        let toggles = this.getToggles(this.$toggleArray, roleId);

        if (toggles != null && toggles.length > 0 && this.isAllStatus(toggles, status)) {
            let toggleAll = this.getFirstToggle(this.$toggleAllArray, roleId);
            this.toggleStatusByValue(toggleAll, status);

            if (isDownline && status == false) {
                this.disableElement(toggleAll);
            }
        }
    }

    isAllStatus = (toggles: HTMLElement[], status: boolean): boolean =>
        toggles.every((toggle) => {
            if (!this.isEnabledElement(toggle)) {
                return true;
            }

            return this.isChecked(toggle) == status;
        });

    disableElement(toggle: HTMLElement) {
        let nextLabel = this.getNextLabel(toggle);
        super.disableElement(nextLabel);
    }

    enableElement(toggle: HTMLElement) {
        let nextLabel = this.getNextLabel(toggle);
        super.enableElement(nextLabel);
    }

    allowActionOnElement(toggle: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(toggle);
        return super.allowActionOnElement(nextLabel);
    }

    isEnabledElement(toggle: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(toggle);
        return super.isEnabledElement(nextLabel);
    }

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, row) => {
            let firstCheckbox = (this.$(row).find('input:checkbox:first').toArray() as HTMLElement[])[0];
            if (this.allowActionOnElement(firstCheckbox)) {
                let data = {
                    sportType: this.getSportType(firstCheckbox),
                    betType: this.getBetType(firstCheckbox),
                    disable: !this.isChecked(firstCheckbox),
                    updateAllMasters: false,
                    disableAllMasters: null,
                    updateAllAgents: false,
                    disableAllAgents: null,
                    updateAllMembers: false,
                    disableAllMembers: null
                };

                (this.$(row).find('input[type=checkbox]').toArray() as HTMLElement[]).forEach((toggle) => {
                    return this.getproductStatus(toggle, data);
                });

                if (data.updateAllMasters || data.updateAllAgents || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        viewModel.productStatus = productStatus;
        viewModel.custIds = this.$('#custIds').val();
        viewModel.custNames = this.$('#custNames').val();

        return viewModel;
    }

    getproductStatus(toggle: HTMLElement, model: any) {
        let disableAll = !this.isEnabledElement(toggle);

        switch (this.getRoleId(toggle)) {
            case 3:
                model.updateAllMasters = true;
                model.disableAllMasters = disableAll;
                break;
            case 2:
                model.updateAllAgents = true;
                model.disableAllAgents = disableAll;
                break;
            case 1:
                model.updateAllMembers = true;
                model.disableAllMembers = disableAll;
                break;
        };

        return model;
    }

    onSuccess(results: any) {
        if (typeof this.$topWindow.onPruductStatusSettingSuccess === "function" && !this.$topWindow.onPruductStatusSettingSuccess(results, this.$.unblockUI)) { return; }

        if (results.length > 0) {
            this.isUpdateSuccess(results) ? super.onSuccess([results[0]]) : this.onError(results);
        }
        else {
            this.showMessage(results.Code, results.Message);
        }
    }

    onError(results: any) {
        let html = [];
        this.$.each(results, function (index, result) {
            html.push('<div class="');
            html.push(result.Code !== 0 ? "alerterror" : "alertsuccess");
            html.push('"><div>');
            html.push(result.CustName);
            html.push(": ");
            html.push(result.Message);
            html.push("</div></div>");
        });

        this.showMixedMessages(html.join(""));
    }

    isUpdateSuccess = (results: any): boolean =>
        results.every((result) => {
            return result.Code == 0;
        });
}

$(function () {
    let productStatusSettingMultipe = new ProductStatusSettingMultipe($, window.top);
    productStatusSettingMultipe.init();
});