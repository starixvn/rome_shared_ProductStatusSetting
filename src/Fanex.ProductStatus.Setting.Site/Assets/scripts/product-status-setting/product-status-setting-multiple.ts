class ProductStatusSettingMultipe {
    $: JQueryStatic;
    $theadButton: HTMLElement[];
    $tbodyButton: HTMLElement[];

    $custId: string;
    $custRole: string;
    $productId: string;
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

        this.$theadButton = thead.find("input[class='checkbox-hide']").toArray();
        this.$tbodyButton = tbody.find("input[class='checkbox-hide']").toArray();

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

    bindingTbodyEvent = (self: any) =>
        this.$tbodyButton.forEach((element) => {
        element.addEventListener('click', () => {
            this.checkboxChanged(this.$(element));
        });
    });

    bindingTheadEvent = (self: any) =>
        this.$theadButton.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleAllStatus(this.$(element));
            });
        });


    bindingSubmitButtonEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetButtonEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    registerEvents = () => {
        this.bindingTheadEvent(this);
        this.bindingTbodyEvent(this);


        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toggleAllStatus = (element: any) => {
        if (this.allowActionOnElement(element)) {
            this.toggleStatus(element);
            this.toggleAllStatusOnCheckbox(element.prop("checked"), this.getRoleId(element));
        }
    }

    toggleAllStatusOnCheckbox = (enableAllStatus: boolean, roleId: number) => {
        let self = this;
        this.$tbodyButton.forEach(function (element, index){
            if (self.getRoleId(element) <= roleId && self.$(element).prop("checked") != enableAllStatus) {
                element.click();
            }
        });
    }

    checkboxChanged = (element: any) => {
       // if (this.isEnablebUpline(element)) {
        this.toggleStatus(element);
        let roleId = this.getRoleId(element);
        this.watchTheadCheckbox(roleId);

        if (roleId > 0)
        {
            let bettype = this.getBetType(element);
            let enable = this.$(element).is(":checked");
            this.triggerDownline(roleId - 1, bettype, enable);
        }
       // }
    }

    triggerDownline = (roleId: number, bettype: number, enable: boolean) => {
        let downlineElement = this.buildElementId(roleId, bettype);
        if (this.$(downlineElement).prop("checked") != enable) {
            this.$(downlineElement).click();
        }
    }

    toggleStatus = (element: HTMLElement) => {
        this.toggleStatusByValue(element, this.$(element).is(":checked"));
    }

    toggleStatusByValue = (element: HTMLElement, enable: boolean) => {
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

    watchTheadCheckbox = (roleId: number) => {
        let tbodyElements = this.getArrayElementByRoleId(this.$tbodyButton, roleId);
        let theadElement = this.getFirstElementByRoleId(this.$theadButton, roleId);

        if (tbodyElements != null && tbodyElements.length > 0)
        {
            if (this.isAllStatus(tbodyElements, true)) {
                this.toggleStatusByValue(theadElement, true);
                return;
            }

            if (this.isAllStatus(tbodyElements, false)) {
                this.toggleStatusByValue(theadElement, false);
                return;
            }
        }
    }

    getArrayElementByRoleId = (elements: HTMLElement[], roleId: number): HTMLElement[] => 
        elements.filter((element, index) => {
            if (this.getRoleId(element) == roleId) {
                return element;
            }
        });

    getFirstElementByRoleId = (elements: HTMLElement[], roleId: number): HTMLElement => {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId) {
                return elements[index];
            }
        }

        return null;
    }

    isAllStatus = (elements: HTMLElement[], status: boolean): boolean => 
        elements.every((element, index) => {
            return this.$(elements[index]).prop("checked") == status;
        });

    disableElement = (element: HTMLElement) => this.$(element).addClass(this.$resources.grayClass);
    enableElement = (element: HTMLElement) => this.$(element).removeClass(this.$resources.grayClass);
    allowActionOnElement = (element: HTMLElement) => !this.$(element).hasClass(this.$resources.grayClass);

    isEnabledElement = (className: string) => className === this.$resources.enabledClass;

    getRoleId = (element: HTMLElement): number => parseInt(this.$(element).data("roleid"));
    isEnablebUpline = (element: HTMLElement) => !this.parseBoolean(this.$(element).data("disabledupline"));

    getBetType = (element: HTMLElement): number => parseInt(this.$(element).data("bettype"));
    getSportType = (element: HTMLElement): number => parseInt(this.$(element).data("sporttype"));
    getTotalStatus = (element: HTMLElement): number => parseInt(this.$(element).data("total"));
    isChangeState = (originValue: number, currentValue: number) => originValue != currentValue;

    buildElementId = ( roleId: number, betType: number) =>
        ["#", roleId, "_", betType].join("");

    parseInt = (value: string) => parseInt(value.replace(/,/g, ""));
    parseBoolean = (value: string) => { return this.$.parseJSON(value.toLowerCase()); }

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