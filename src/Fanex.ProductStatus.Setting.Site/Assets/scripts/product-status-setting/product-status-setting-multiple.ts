class ProductStatusSettingMultipe extends ProductStatusSettingBase{
    $: JQueryStatic;
    $theadButton: HTMLElement[];
    $tbodyButton: HTMLElement[];

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

        this.$theadButton = this.$thead.find("input[class='checkbox-hide']").toArray() as HTMLElement[];
        this.$tbodyButton = this.$tbody.find("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$resources.enabled = "Enable All";
        this.$resources.disabled = "Disable All";
        this.$resources.updateActionName = "MultipleUpdate";
    }

    bindingTbodyEvent = () =>
        this.$tbodyButton.forEach((element) => {
            element.addEventListener('click', () => {
                this.toogleBodyCheckbox(element);
        });
    });

    bindingTheadEvent = () =>
        this.$theadButton.forEach((element) => {
            element.addEventListener('click', () => {
                this.toogleHeaderCheckbox(element);
            });
        });

    bindingSubmitButtonEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetButtonEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    registerEvents() {
        this.loadMultiple();

        this.bindingTheadEvent();
        this.bindingTbodyEvent();
        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toogleHeaderCheckbox(element: HTMLElement) {
        if (this.allowActionOnElement(element)) {
            this.toggleStatus(element);

            let roleId = this.getRoleId(element);
            let enable = this.isChecked(element);

            let tbodyElements = this.getArrayElement(this.$tbodyButton, roleId);
            tbodyElements.forEach((el) => {
                if (this.allowActionOnElement(el) && this.isChecked(el) != enable) {
                    el.click();
                }
            });
        }    
    }

    toogleBodyCheckbox(element: HTMLElement) {
        if (this.allowActionOnElement(element)) {
            this.toggleStatus(element);
            let roleId = this.getRoleId(element);

            this.watchHeader(roleId);

            let bettype = this.getBetType(element);
            let enable = this.isChecked(element);
            let downlineElement = this.getBodyElement(this.$tbodyButton, roleId - 1, bettype);

            this.toogleBodyCheckboxDownline(downlineElement, enable);
        }
    }

    toogleBodyCheckboxDownline(element: HTMLElement, enabledUpline: boolean) {
        this.toggleStatusByValue(element, enabledUpline);
        enabledUpline ? this.enableElement(element) : this.disableElement(element);

        let roleId = this.getRoleId(element);
        this.watchHeader(roleId);

        if (roleId > 1) {
            let bettype = this.getBetType(element);
            let enable = this.isChecked(element);
            let downlineElement = this.getBodyElement(this.$tbodyButton, roleId - 1, bettype);

            this.toogleBodyCheckboxDownline(downlineElement, enable);
        }
    }

    getBodyElement(elements: HTMLElement[], roleId: number, bettype: number): HTMLElement {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId && this.getBetType(elements[index]) == bettype) {
                return elements[index];
            }
        }

        return null;
    }

    toggleStatus = (element: HTMLElement) => this.toggleStatusByValue(element, this.isChecked(element));

    toggleStatusByValue = (element: HTMLElement, enable: boolean) => {
        this.$(element).prop('checked', enable);
        let labelElement = this.getNextLabel(element);

        let tooltip = enable ? this.$(labelElement).data("disabletooltip") : this.$(labelElement).data("enabletooltip")
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;
        let labelClass = enable ? this.$resources.enabledClass : this.$resources.disabledClass;
        let bettype = this.getBetType(element);

        this.$(labelElement).attr("title", tooltip)
            .removeClass()
            .addClass(labelClass)
            .text(labelText);
    }

    watchHeader = (roleId: number) => {
        if (this.$isMultipleBetTypes) {
            [true, false].forEach((status) => {
                this.toogleHeader(roleId, status);
            });
        }
    }

    toogleHeader = (roleId: number, status: boolean) => {
        let tbodyElements = this.getArrayElement(this.$tbodyButton, roleId);

        if (tbodyElements != null && tbodyElements.length > 0 && this.isAllStatus(tbodyElements, status)) {
            let theadElement = this.getFirstElement(this.$theadButton, roleId);
            this.toggleStatusByValue(theadElement, status); 
        }
    }

    getArrayElement = (elements: HTMLElement[], roleId: number): HTMLElement[] => 
        elements.filter((element, index) => {
            if (this.getRoleId(element) == roleId) {
                return element;
            }
        });

    getFirstElement = (elements: HTMLElement[], roleId: number): HTMLElement => {
        let arrayElement = this.getArrayElement(elements, roleId);
        return arrayElement.length > 0 ? arrayElement[0] : null;
    }

    isAllStatus = (elements: HTMLElement[], status: boolean): boolean => 
        elements.every((element, index) => {
            return this.isChecked(elements[index]) == status;
        });

    disableElement(element: HTMLElement) {
        let nextLabel = this.getNextLabel(element);
        super.disableElement(nextLabel);
    }

    enableElement(element: HTMLElement) {
        let nextLabel = this.getNextLabel(element);
        super.enableElement(nextLabel);
    }

    allowActionOnElement(element: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(element);
        return super.allowActionOnElement(nextLabel);
    }

    isEnabledElement(element: HTMLElement): boolean {
        let nextLabel = this.getNextLabel(element);
        return super.isEnabledElement(nextLabel);
    }

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, columnElement) => {
            let checkboxElement = this.$(columnElement).find("input[type=checkbox]").toArray()[0] as HTMLElement;
            if (this.allowActionOnElement(checkboxElement)) {
                let bettype = this.getBetType(checkboxElement);
                let sporttype = this.getSportType(checkboxElement);
               
                let data = {
                    sportType: sporttype,
                    betType: bettype,
                    disable: !this.isChecked(checkboxElement),
                    updateAllMasters: false,
                    disableAllMasters: null,
                    updateAllAgents: false,
                    disableAllAgents: null,
                    updateAllMembers: false,
                    disableAllMembers: null
                };

                this.$(columnElement).find('input[type=checkbox]').each((index, element) => {
                    return this.getproductStatus(element, data);
                });

                if (data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        viewModel.productStatus = productStatus;
        viewModel.custIds = this.$('#CustIds').val();
        viewModel.custNames = this.$('#CustNames').val();
        return viewModel;
    }

    getproductStatus(disabledSpan: any, model: any) {
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

    onSuccess = (results: any) => {
        if (this.isUpdateSuccess(results))
        {
            this.showMessage(results[0].Code, results[0].Message);

            setTimeout(function () {
                this.$.unblockUI();
                location.reload(true);
            }, this.$timeoutDefault);
        }
        else
        {
            let html: string;
            results.forEach((result) => {
                if (result.Code != 0) {
                    html +=  [result.CustName, ": ", result.Message, "<br>"].join("");
                }
            });

            this.showMessage(this.$errorCodeDefault, html);
        }   
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