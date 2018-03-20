class ProductStatusSettingMultipe {
    $: JQueryStatic;
    $theadButton: HTMLInputElement[];
    $tbodyButton: HTMLInputElement[];

    $custId: number;
    $custRole: number;
    $productId: number;
    $resources: any;
    $errorCodeDefault: number;
    $timeoutDefault: number;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();

        this.$custId = this.$('#custId').val();
        this.$custRole = this.$('#custRole').val();
        this.$productId = this.$('#productId').val();

        this.$errorCodeDefault = 101;
        this.$timeoutDefault = 3000;
    }

    loadMultiple = () => {
        let selft = this;
        this.$("#Products").change(() => {
            this.$("#ProductId").val(this.$("#Products").val());
            this.$("#product-form").submit();
        });
    }

    cachedElements = () => {
        let table = this.$("#tbl-product");
        let thead = table.find("thead");
        let tbody = table.find("tbody");

        this.$theadButton = thead.find("input[class='checkbox-hide']").toArray() as HTMLInputElement[];
        this.$tbodyButton = tbody.find("input[class='checkbox-hide']").toArray() as HTMLInputElement[];

        this.$resources = {
            enabled: "Enable All",
            disabled: "Disable All",
            updateStatusFailed: this.$('#updateStatusFailed').val(),
            thereIsNoChange: this.$('#thereIsNoChange').val(),
            enabledClass: "enabled",
            disabledClass: "disabled",
            grayClass: "gray"
        }
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

    registerEvents = () => {
        this.bindingTheadEvent();
        this.bindingTbodyEvent();
        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();

        this.loadMultiple();
    }

    toogleHeaderCheckbox = (element: HTMLInputElement) => {
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

    toogleBodyCheckbox = (element: HTMLInputElement) => {
        if (this.allowActionOnElement(element)) {
            this.toggleStatus(element);
            let roleId = this.getRoleId(element);

            [true, false].forEach((status) => {
                this.watchHeader(roleId, status);
            });


                let bettype = this.getBetType(element);
                let enable = this.isChecked(element);
                let downlineElement = this.getBodyElement(this.$tbodyButton, roleId - 1, bettype);

                this.toogleBodyCheckboxDownline(downlineElement, enable);
        }
    }

    toogleBodyCheckboxDownline = (element: HTMLInputElement, enabledUpline: boolean) => {
           this.toggleStatusByValue(element, enabledUpline);
            enabledUpline ? this.enableElement(element) : this.disableElement(element);

            let roleId = this.getRoleId(element);

            [true, false].forEach((status) => {
                this.watchHeader(roleId, status);
            });

            if (roleId > 1) {
                let bettype = this.getBetType(element);
                let enable = this.isChecked(element);
                let downlineElement = this.getBodyElement(this.$tbodyButton, roleId - 1, bettype);

                this.toogleBodyCheckboxDownline(downlineElement, enable);
            }
        
    }

    getBodyElement = (elements: HTMLInputElement[], roleId: number, bettype: number): HTMLInputElement =>
    {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId && this.getBetType(elements[index]) == bettype) {
                return elements[index];
            }
        }

        return null;
    }

    toggleStatus = (element: HTMLInputElement) => this.toggleStatusByValue(element, this.isChecked(element));

    toggleStatusByValue = (element: HTMLInputElement, enable: boolean) => {
        this.$(element).prop('checked', enable);
        let labelElement = this.$(element).parent().find("label");

        let tooltip = enable ? labelElement.data("disabletooltip") : labelElement.data("enabletooltip")
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;
        let labelClass = enable ? this.$resources.enabledClass : this.$resources.disabledClass;
        let bettype = this.getBetType(element);

        labelElement.attr("title", tooltip)
            .removeClass()
            .addClass(labelClass)
            .text(labelText);
    }

    watchHeader = (roleId: number, status: boolean) => {
        let tbodyElements = this.getArrayElement(this.$tbodyButton, roleId);

        if (tbodyElements != null && tbodyElements.length > 0 && this.isAllStatus(tbodyElements, status)) {
            let theadElement = this.getFirstElement(this.$theadButton, roleId);
            this.toggleStatusByValue(theadElement, status); 
        }
    }

    getArrayElement = (elements: HTMLInputElement[], roleId: number): HTMLInputElement[] => 
        elements.filter((element, index) => {
            if (this.getRoleId(element) == roleId) {
                return element;
            }
        });

    getFirstElement = (elements: HTMLInputElement[], roleId: number): HTMLInputElement => {
        let arrayElement = this.getArrayElement(elements, roleId);
        let defaultElement: HTMLInputElement;
        return arrayElement.length > 0 ? arrayElement[0] : null;
    }

    isAllStatus = (elements: HTMLInputElement[], status: boolean): boolean => 
        elements.every((element, index) => {
            return this.isChecked(elements[index]) == status;
        });


    disableElement = (element: HTMLInputElement) => {
        let nextLabel = this.getNextLable(element);
        this.$(nextLabel).addClass(this.$resources.grayClass);
    }

    enableElement = (element: HTMLInputElement) => {
        let nextLabel = this.getNextLable(element);
        this.$(nextLabel).removeClass(this.$resources.grayClass);
    }

    allowActionOnElement = (element: HTMLInputElement): boolean => {
        let nextLabel = this.getNextLable(element);
        return !this.$(nextLabel).hasClass(this.$resources.grayClass) && this.isEnablebUpline(element);
    }

    getNextLable = (element: HTMLInputElement) => this.$(element).parent().find("label");

    isEnabledElement = (element: HTMLInputElement): boolean =>
    {
        let nextLabel = this.getNextLable(element);
        return this.$(nextLabel).hasClass(this.$resources.enabledClass);
    }
        
    isEnablebUpline = (element: HTMLElement): boolean =>
        !this.parseBoolean(this.$(element).data("disabledupline"));

    isChangeState = (originValue: number, currentValue: number): boolean =>
        originValue != currentValue;

    isChecked = (element: HTMLInputElement): boolean =>
        this.$(element).is(":checked");

    getBetType = (element: HTMLInputElement): number =>
        parseInt(this.$(element).data("bettype"));

    getSportType = (element: HTMLInputElement): number =>
        parseInt(this.$(element).data("sporttype"));

    getTotalStatus = (element: HTMLInputElement): number =>
        parseInt(this.$(element).data("total"));

    getRoleId = (element: HTMLInputElement): number =>
        parseInt(this.$(element).data("roleid"));

    parseInt = (value: string): number =>
        parseInt(value.replace(/,/g, ""));

    parseBoolean = (value: string): boolean =>
        this.$.parseJSON(value.toLowerCase()); 

    setupAntiXSRF = () => {
        let token = this.$('[name=__RequestVerificationToken]').val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    onSubmit = (e: any) => {
        e.preventDefault();

        let headers = this.setupAntiXSRF();
        let updateViewModel = this.getUpdateViewModel();

        this.$.blockUI({
            blockMsgClass: "block-ui-loading"
        });

        updateViewModel.productStatus.length > 0 ?
            this.callAjax(headers, updateViewModel) :
            this.showMessage(this.$errorCodeDefault, this.$resources.thereIsNoChange);
    }

    callAjax = (headers: any, updateViewModel: any) =>
        this.$.ajax({
            type: "POST",
            url: "MultipleUpdate",
            cache: false,
            headers: headers,
            data: JSON.stringify(updateViewModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: this.onSuccess,
            error: this.onError
        })

    getUpdateViewModel = () => {
        let self = this;

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, columnElement) => {
            let checkboxElement = this.$(columnElement).find("input[type=checkbox]").toArray()[0] as HTMLInputElement;
            if (self.allowActionOnElement(checkboxElement)) {
                let bettype = self.getBetType(checkboxElement);
                let sporttype = self.getSportType(checkboxElement);
               
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

                this.$(columnElement).find('input[type=checkbox]').each(function () {
                    return self.getproductStatus(this, data);
                });

                if (data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        return {
            productId: this.$productId,
            custIds: this.getCustIds(),
            custNames: this.$('#CustNames').val().split(','),
            productStatus: productStatus
        };
    }

    getCustIds = () => {
        return this.$('[name="CustIds"]').map((index, element) => {
            return this.$(element).val();
        }).get();
    }


    getproductStatus = (disabledSpan: any, model: any) =>{
        let disableAll = this.isEnabledElement(disabledSpan);

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
            let html = '';
            results.forEach((result) => {
                if (result.Code != 0) {
                    html +=  [result.CustName, ": ", result.Message, "<br>"].join("");
                }
            });

            this.showMessage(this.$errorCodeDefault, html);
        }   
    }

    isUpdateSuccess = (results: any) => 
        results.every((result) => {
            return result.Code == 0;
        })

    onError = () => this.showMessage(this.$errorCodeDefault, this.$resources.updateStatusFailed);

    showMessage = (type: any, message: string) => {
        this.$.unblockUI();
        let html = ['<div class="', type === 0 ? "alertsuccess" : "alerterror", '"><div>', message, "</div></div>"].join("");
        let messageDiv = this.$("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }

    hideMessage = () => this.$("#sectionMessage").html("").addClass("hide");
}

$(function () {
    let productStatusSettingMultipe = new ProductStatusSettingMultipe($);
    productStatusSettingMultipe.init();
});