class ProductStatusSettingMultipe extends ProductStatusSettingBase{
    $: JQueryStatic;
    $theadCheckbox: HTMLElement[];
    $tbodyCheckbox: HTMLElement[];

    constructor($: JQueryStatic) {
        super($);
    }

    init() {
        super.init();
        this.registerEvents();
    }

    loadMultiple = () => {
        this.$("#Products").change(() => {
            this.$("#ProductId").val(this.$("#Products").val());
            this.$("#product-form").submit();
        });
    }

    cachedElements() {
        super.cachedElements();

        this.$theadCheckbox = this.$thead.find("input[class='checkbox-hide']").toArray() as HTMLElement[];
        this.$tbodyCheckbox = this.$tbody.find("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enableAll').val();
        this.$resources.disabled = this.$('#disableAll').val();
        this.$resources.updateActionName = "MultipleUpdate";
    }

    bindingTbodyEvent = () =>
        this.$tbodyCheckbox.forEach((bodyChk) => {
            bodyChk.addEventListener('click', () => {
                this.toogleBodyCheckbox(bodyChk);
        });
    });

    bindingTheadEvent = () =>
        this.$theadCheckbox.forEach((headerChk) => {
            headerChk.addEventListener('click', () => {
                this.toogleHeaderCheckbox(headerChk);
            });
        });

    registerEvents() {
        this.loadMultiple();

        this.bindingTheadEvent();
        this.bindingTbodyEvent();
        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toogleHeaderCheckbox(headerCheckbox: HTMLElement) {
        if (this.allowActionOnElement(headerCheckbox)) {
            this.toggleStatus(headerCheckbox);

            let roleId = this.getRoleId(headerCheckbox);
            let enable = this.isChecked(headerCheckbox);

            let tbodyElements = this.getArrayElement(this.$tbodyCheckbox, roleId);
            tbodyElements.forEach((bodyCheckbox) => {
                if (this.allowActionOnElement(bodyCheckbox) && this.isChecked(bodyCheckbox) != enable) {
                    bodyCheckbox.click();
                }
            });
        }    
    }

    toogleBodyCheckbox(bodyCheckbox: HTMLElement) {
        if (this.allowActionOnElement(bodyCheckbox)) {
            this.toggleStatus(bodyCheckbox);
            let roleId = this.getRoleId(bodyCheckbox);

            this.watchHeader(roleId, false);

            let bettype = this.getBetType(bodyCheckbox);
            let enable = this.isChecked(bodyCheckbox);
            let downlineCheckbox = this.getBodyElement(this.$tbodyCheckbox, roleId - 1, bettype);

            this.toogleBodyCheckboxDownline(downlineCheckbox, enable);
        }
    }

    toogleBodyCheckboxDownline(bodyCheckbox: HTMLElement, enabledUpline: boolean) {
        this.toggleStatusByValue(bodyCheckbox, enabledUpline);
        enabledUpline ? this.enableElement(bodyCheckbox) : this.disableElement(bodyCheckbox);

        let roleId = this.getRoleId(bodyCheckbox);
        this.watchHeader(roleId, true);

        if (roleId > 1) {
            let bettype = this.getBetType(bodyCheckbox);
            let enable = this.isChecked(bodyCheckbox);
            let downlineCheckbox = this.getBodyElement(this.$tbodyCheckbox, roleId - 1, bettype);

            this.toogleBodyCheckboxDownline(downlineCheckbox, enable);
        }
    }

    getBodyElement(tbodyCheckbox: HTMLElement[], roleId: number, bettype: number): HTMLElement {
        for (let index = 0; index < tbodyCheckbox.length; index++) {
            let bodyCheckbox = tbodyCheckbox[index];
            if (this.getRoleId(bodyCheckbox) == roleId && this.getBetType(bodyCheckbox) == bettype) {
                return bodyCheckbox;
            }
        }

        return null;
    }

    toggleStatus = (checkboxElement: HTMLElement) => this.toggleStatusByValue(checkboxElement, this.isChecked(checkboxElement));

    toggleStatusByValue = (checkboxElement: HTMLElement, enable: boolean) => {
        this.$(checkboxElement).prop('checked', enable);
        let labelElement = this.getNextLabel(checkboxElement);

        let tooltip = enable ? this.$(labelElement).data("disabletooltip") : this.$(labelElement).data("enabletooltip")
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;
        let labelClass = enable ? this.$resources.enabledClass : this.$resources.disabledClass;
        let bettype = this.getBetType(checkboxElement);

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
        let tbodyCheckbox = this.getArrayElement(this.$tbodyCheckbox, roleId);

        if (tbodyCheckbox != null && tbodyCheckbox.length > 0 && this.isAllStatus(tbodyCheckbox, status)) {
            let theadCheckbox = this.getFirstElement(this.$theadCheckbox, roleId);
            this.toggleStatusByValue(theadCheckbox, status); 

            if (isDownline && status == false)
            {
                this.disableElement(theadCheckbox);
            }
        }
    }

    isAllStatus = (tbodyCheckbox: HTMLElement[], status: boolean): boolean => 
        tbodyCheckbox.every((bodyCheckbox) => {
            if (!this.isEnabledElement(bodyCheckbox))
            {
                return true;
            }

            return this.isChecked(bodyCheckbox) == status;
        });

    disableElement(checkboxElement: HTMLElement) {
        let nextLabel = this.getNextLabel(checkboxElement);
        super.disableElement(nextLabel);
    }

    enableElement(checkboxElement: HTMLElement) {
        let nextLabel = this.getNextLabel(checkboxElement);
        super.enableElement(nextLabel);
    }

    allowActionOnElement(checkboxElement: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(checkboxElement);
        return super.allowActionOnElement(nextLabel);
    }

    isEnabledElement(checkboxElement: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(checkboxElement);
        return super.isEnabledElement(nextLabel);
    }

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, row) => {
            let firstCheckbox = (this.$(row).find('input:checkbox:first').toArray() as HTMLElement[])[0];
            if (this.allowActionOnElement(firstCheckbox)) {
                let bettype = this.getBetType(firstCheckbox);
                let sporttype = this.getSportType(firstCheckbox);
               
                let data = {
                    sportType: sporttype,
                    betType: bettype,
                    disable: !this.isChecked(firstCheckbox),
                    updateAllMasters: false,
                    disableAllMasters: null,
                    updateAllAgents: false,
                    disableAllAgents: null,
                    updateAllMembers: false,
                    disableAllMembers: null
                };

                (this.$(row).find('span:odd').toArray() as HTMLElement[]).forEach((spanElement) => {
                    return this.getproductStatus(spanElement, data);
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

    getproductStatus(disabledSpan: HTMLElement, model: any) {
        let disableAll = !this.isEnabledElement(disabledSpan);

        switch (this.getRoleId(disabledSpan)) {
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