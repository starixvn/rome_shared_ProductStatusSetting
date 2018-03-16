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
                this.toogleBodyCheckbox(element, false);
        });
    });

    bindingTheadEvent = () =>
        this.$theadButton.forEach((element) => {
            element.addEventListener('click', () => {
                this.toogleHeaderCheckbox(element, false);
            });
        });


    bindingSubmitButtonEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetButtonEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    registerEvents = () => {
        this.bindingTheadEvent();
        this.bindingTbodyEvent();
        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toogleHeaderCheckbox = (element: HTMLInputElement, toogleByUpline: boolean, enabledUpline?: boolean) => {
        let allowChange = toogleByUpline ? true : this.allowActionOnElement(element);
        if (allowChange) {
            toogleByUpline ? this.toggleStatusByValue(element, enabledUpline) : this.toggleStatus(element);

            let roleId = this.getRoleId(element);
            let enable = this.isChecked(element);

            let tbodyElements = this.getArrayElementByRoleId(this.$tbodyButton, roleId);
            tbodyElements.forEach((el) => {
                this.toogleBodyCheckboxByHeader(el, enable);
            });
        
            if (roleId > 1) {
                let downlineHeader = this.getFirstElementByRoleId(this.$theadButton, roleId - 1);
                let enable = this.isChecked(element);
                this.toogleHeaderCheckbox(downlineHeader, true, enable);
                enable ? this.enableElement(downlineHeader) : this.disableElement(downlineHeader);
            }
        }
    }

    toogleBodyCheckbox = (element: HTMLInputElement, toogleByUpline: boolean,  enabledUpline?: boolean) => {
        //if (this.isEnablebUpline(element)) {
        if (this.allowActionOnElement(element)) {
            toogleByUpline ? this.toggleStatusByValue(element, enabledUpline) : this.toggleStatus(element);

            let roleId = this.getRoleId(element);

            [true, false].forEach((status) => {
                this.watchHeader(roleId, status);
            });

            if (roleId > 0) {
                let bettype = this.getBetType(element);
                let enable = this.isChecked(element);
                let downlineElement = this.getDownlineElement(this.$tbodyButton, roleId - 1, bettype);

                this.toogleBodyCheckbox(downlineElement, true, enable);
            }
        }
    }

    toogleBodyCheckboxByHeader = (element: HTMLInputElement, allowChange: boolean) => {
        this.toggleStatusByValue(element, allowChange);
        allowChange ? this.toggleStatusByValue(element, allowChange) : this.disableElement(element);
    }

    getDownlineElement = (elements: HTMLInputElement[], roleId: number, bettype: number): HTMLInputElement =>
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
        let tbodyElements = this.getArrayElementByRoleId(this.$tbodyButton, roleId);

        if (tbodyElements != null && tbodyElements.length > 0 && this.isAllStatus(tbodyElements, status)) {
            let theadElement = this.getFirstElementByRoleId(this.$theadButton, roleId);
            this.toggleStatusByValue(theadElement, status); 
        }
    }

    getArrayElementByRoleId = (elements: HTMLInputElement[], roleId: number): HTMLInputElement[] => 
        elements.filter((element, index) => {
            if (this.getRoleId(element) == roleId) {
                return element;
            }
        });

    getFirstElementByRoleId = (elements: HTMLInputElement[], roleId: number): HTMLInputElement => {
        let arrayElement = this.getArrayElementByRoleId(elements, roleId);
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
        return !this.$(nextLabel).hasClass(this.$resources.grayClass);
    }

    getNextLable = (element: HTMLInputElement) => 
        this.$(element).parent().find("label");

    isEnabledElement = (className: string): boolean =>
        className === this.$resources.enabledClass;

    isEnablebUpline = (element: HTMLInputElement): boolean =>
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
            url: "Update",
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

        let productStatus = self.$('#tbl-product > tbody  > tr').map(function () {
            let checkboxStatus = self.$(this).find('input:checkbox');
            if (self.isEnablebUpline(this)) {
                let bettype = self.getBetType(this);
                let sporttype = self.getSportType(this);
                let disable = !self.$(checkboxStatus).prop("checked");
                let originstatus = self.parseBoolean(checkboxStatus.data("originstatus"));

                let data = {
                    sportType: sporttype,
                    betType: bettype,
                    disable: disable,
                    updateAllMasters: false,
                    disableAllMasters: null,
                    updateAllAgents: false,
                    disableAllAgents: null,
                    updateAllMembers: false,
                    disableAllMembers: null
                };

                self.$(this).find('span:odd').each(function () {
                    return self.getproductStatus(this, data);
                });

                if (data.disable != originstatus
                    || data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        return {
            productId: this.$productId,
            custId: this.$custId,
            productStatus: productStatus
        };
    }

    getproductStatus = (disabledSpan: any, model: any) =>{
        let levelStatus = this.getLevelStatus(this.$(disabledSpan));

        switch (this.getRoleId(disabledSpan)) {
            case 3:
                model.updateAllMasters = levelStatus.UpdateAll;
                model.disableAllMasters = levelStatus.DisableAll;
                break;
            case 2:
                model.updateAllAgents = levelStatus.UpdateAll;
                model.disableAllAgents = levelStatus.DisableAll;
                break;
            case 1:
                model.updateAllMembers = levelStatus.UpdateAll;
                model.disableAllMembers = levelStatus.DisableAll;
                break;
        };

        return model;
    }

    getLevelStatus = (disabledSpan: any) => {
        let totaldisabled = this.parseInt(disabledSpan.text());
        let originalTotaldisabled = disabledSpan.data("totaldisabled");
        let updateAll = this.allowActionOnElement(disabledSpan) && this.isChangeState(originalTotaldisabled, totaldisabled);

        return {
            UpdateAll: updateAll,
            DisableAll: updateAll && totaldisabled > 0
        }
    }

    onSuccess = (data: any) => {
        this.showMessage(data.Code, data.Message);
        if (data.Code == 0) { 
            setTimeout(function () {
                this.$.unblockUI();
                location.reload(true);
            }, this.$timeoutDefault);
        }
    }

    onError = () => this.showMessage(this.$errorCodeDefault, this.$resources.updateStatusFailed);

    showMessage = (type: number, message: string) => {
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