class ProductStatusSettingMultipe extends ProductStatusSettingBase{
    $: JQueryStatic;
    $toggleAllCheckboxes: HTMLElement[];
    $toggleCheckboxes: HTMLElement[];

    constructor($: JQueryStatic) {
        super($);
    }

    init() {
        super.init();
        this.registerEvents();
    }

    loadMultiple = () => 
        this.$("#Products").change(() => {
            this.$("#ProductId").val(this.$("#Products").val());
            this.$("#product-form").submit();
        });

    cachedElements() {
        super.cachedElements();

        this.$toggleAllCheckboxes = this.$thead.find("input[class='checkbox-hide']").toArray() as HTMLElement[];
        this.$toggleCheckboxes = this.$tbody.find("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enableAll').val();
        this.$resources.disabled = this.$('#disableAll').val();
        this.$resources.updateActionName = "MultipleUpdate";
    }

    bindingToggleEvent = () =>
        this.$toggleCheckboxes.forEach((element) => {
            element.addEventListener('click', () => {
                this.toogleStatus(element);
        });
    });

    bindingToggleAllEvent = () =>
        this.$toggleAllCheckboxes.forEach((element) => {
            element.addEventListener('click', () => {
                this.toogleStatusAll(element);
            });
        });

    registerEvents() {
        this.loadMultiple();

        this.bindingToggleAllEvent();
        this.bindingToggleEvent();
        this.bindingResetEvent();
        this.bindingSubmitEvent();
    }

    toogleStatusAll(toogleAll: HTMLElement) {
        if (this.allowActionOnElement(toogleAll)) {
            this.toggleStatus(toogleAll);

            let roleId = this.getRoleId(toogleAll);
            let enable = this.isChecked(toogleAll);

            let toogles = this.getElements(this.$toggleCheckboxes, roleId);
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

            let bettype = this.getBetType(toogle);
            let enable = this.isChecked(toogle);
            let downlineToggle = this.getToggleElements(this.$toggleCheckboxes, roleId - 1, bettype);

            this.toogleStatusDownline(downlineToggle, enable);
        }
    }

    toogleStatusDownline(toogle: HTMLElement, enabledUpline: boolean) {
        this.toggleStatusByValue(toogle, enabledUpline);
        enabledUpline ? this.enableElement(toogle) : this.disableElement(toogle);

        let roleId = this.getRoleId(toogle);
        this.watchHeader(roleId, true);

        if (roleId > 1) {
            let bettype = this.getBetType(toogle);
            let enable = this.isChecked(toogle);
            let downlineToggle = this.getToggleElements(this.$toggleCheckboxes, roleId - 1, bettype);

            this.toogleStatusDownline(downlineToggle, enable);
        }
    }

    getToggleElements(toggleCheckboxes: HTMLElement[], roleId: number, bettype: number): HTMLElement {
        for (let index = 0; index < toggleCheckboxes.length; index++) {
            let toggle = toggleCheckboxes[index];
            if (this.getRoleId(toggle) == roleId && this.getBetType(toggle) == bettype) {
                return toggle;
            }
        }

        return null;
    }

    toggleStatus = (toggle: HTMLElement) => this.toggleStatusByValue(toggle, this.isChecked(toggle));

    toggleStatusByValue = (toggle: HTMLElement, enable: boolean) => {
        this.$(toggle).prop('checked', enable);
        let labelElement = this.getNextLabel(toggle);

        let tooltip = enable ? this.$(labelElement).data("disabletooltip") : this.$(labelElement).data("enabletooltip")
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;
        let labelClass = enable ? this.$resources.enabledClass : this.$resources.disabledClass;
        let bettype = this.getBetType(toggle);

        this.$(labelElement).attr("title", tooltip)
            .removeClass()
            .addClass(labelClass)
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
        let toggles = this.getElements(this.$toggleCheckboxes, roleId);

        if (toggles != null && toggles.length > 0 && this.isAllStatus(toggles, status)) {
            let toggleAll = this.getFirstElement(this.$toggleAllCheckboxes, roleId);
            this.toggleStatusByValue(toggleAll, status); 

            if (isDownline && status == false)
            {
                this.disableElement(toggleAll);
            }
        }
    }

    isAllStatus = (toggles: HTMLElement[], status: boolean): boolean => 
        toggles.every((toggle) => {
            if (!this.isEnabledElement(toggle))
            {
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

                if (data.updateAllMasters|| data.updateAllAgents || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        viewModel.productStatus = productStatus;
        viewModel.custIds = this.$('#CustIds').val();
        viewModel.custNames = this.$('#CustNames').val();

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
        this.isUpdateSuccess(results) ? super.onSuccess(results[0]) : this.onError(results);
    }

    onError(results: any) {
        let html: string;
        results.forEach((result) => {
            if (result.Code != 0) {
                html += [result.CustName, ": ", result.Message, "<br>"].join("");
            }
        });

        super.showMessage(this.$errorCodeDefault, html);
    }

    isUpdateSuccess = (results: any): boolean =>
        results.every((result) => {
            return result.Code == 0;
        });
}

$(function () {
    let productStatusSettingMultipe = new ProductStatusSettingMultipe($);
    productStatusSettingMultipe.init();
});