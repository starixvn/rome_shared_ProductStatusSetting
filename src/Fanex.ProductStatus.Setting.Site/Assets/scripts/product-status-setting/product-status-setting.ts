class ProductStatusSetting {
    $: JQueryStatic;
    $tbodyEnabledSpans: JQuery;
    $tbodyDisabledSpans: JQuery;
    $checkboxHidden: JQuery;
    $theadEnableSpan: JQuery;
    $theadDisabledSpan: JQuery;

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

        this.$checkboxHidden = this.$("input[class='checkbox-hide']");
        this.$tbodyEnabledSpans = tbody.find("span.enabled");
        this.$tbodyDisabledSpans = tbody.find("span.disabled");
        this.$theadEnableSpan = thead.find("span.enabled");
        this.$theadDisabledSpan = thead.find("span.disabled");

        this.$resources = {
            enabled: this.$('#enabled').val(),
            disabled: this.$('#disabled').val(),
            updateStatusFailed: this.$('#updateStatusFailed').val(),
            thereIsNoChange: this.$('#thereIsNoChange').val(),
            enabledClass: "enabled",
            disabledClass: "disabled",
            grayClass: "gray"
        }
    }

    bindingCheckboxHiddenEvent = (self: any) =>
        self.$checkboxHidden
            .unbind("change")
            .bind("change", function () {
                self.checkboxChanged(self.$(this));
            });

    bindingEnableSpanEvent = (self: any) =>
        self.$tbodyEnabledSpans
            .unbind("click")
            .bind("click", function () {
                self.toggleStatusBySpan(self.$(this));
            });

    bindingDisableSpanEvent = (self: any) =>
        self.$tbodyDisabledSpans
            .unbind("click")
            .bind("click", function () {
                self.toggleStatusBySpan(self.$(this));
            });

    bindingTheadEnableSpanEvent = (self: any) =>
        self.$theadEnableSpan
            .unbind("click")
            .bind("click", function () {
                self.toggleAllStatus(self.$(this), true);
            });

    bindingTheadDisableSpanEvent = (self: any) =>
        self.$theadDisabledSpan
            .unbind("click")
            .bind("click", function () {
                self.toggleAllStatus(self.$(this), false);
            });

    bindingSubmitButtonEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetButtonEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    registerEvents = () => {
        this.bindingCheckboxHiddenEvent(this);
        this.bindingEnableSpanEvent(this);
        this.bindingDisableSpanEvent(this);
        this.bindingTheadEnableSpanEvent(this);
        this.bindingTheadDisableSpanEvent(this);

        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toggleAllStatus = (element: any, enable: boolean) => {
        if (this.allowActionOnElement(element)) {
            let roleId = this.getRoleId(element);
            this.$custRole == roleId ? this.toggleAllStatusOnCheckbox(enable) : this.toggleAllStatusOnSpans(roleId, enable);
        }
    }

    toggleAllStatusOnSpans = (toggleAllRoleId: number, enable: boolean) => {
        let self = this;
        let statusSpanArray = enable ? this.$tbodyEnabledSpans : this.$tbodyDisabledSpans;

        statusSpanArray.each(function (index, element) {
            let statusSpan = self.$(element);
            if (self.getRoleId(statusSpan) === toggleAllRoleId) {
                statusSpan.click();
            }
        });
    }

    toggleAllStatusOnCheckbox = (enableAllStatus: boolean) => {
        let self = this;
        this.$checkboxHidden.each(function (index, element) {
            let chkElement = self.$(element);
            if (chkElement.prop("checked") != enableAllStatus) {
                chkElement.click();
            }
        });
    }

    checkboxChanged = (element: any) => {
        let enable = this.$(element).is(":checked");
        let labelElement = element.next("label");

        let tooltip = enable ? labelElement.data("disabletooltip") : labelElement.data("enabletooltip")
        let labelText = enable ? this.$resources.enabled : this.$resources.disabled;

        labelElement.attr("title", tooltip)
            .removeClass()
            .addClass(labelText.toLowerCase())
            .text(labelText);

        this.toggleStatusDownlines(this.getRoleId(element) - 1, this.$(element).is(":checked"), this.getBetType(element));
    }

    toggleStatusBySpan = (statusSpan: any) => this.toggleStatus(statusSpan, this.isEnabledElement(statusSpan.attr("class")));

    toggleStatus = (statusSpan: any, enable: boolean) => {
        if (this.allowActionOnElement(statusSpan)) {
            let roleId = this.getRoleId(statusSpan);
            let betType = this.getBetType(statusSpan);

            let oppositeClassName = enable ? this.$resources.disabledClass : this.$resources.enabledClass;
            let oppositeId = this.buildElementId(oppositeClassName, roleId, betType);
            let oppositeStatusSpan = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatusSpan.text());

            if (oppositeTotal > 0) {
                oppositeStatusSpan.text("0");
                statusSpan.text(this.getTotalStatus(statusSpan));
            }

            this.toggleStatusDownlines(roleId - 1, enable, betType);
        }
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0) {
            let enabledSpanId = this.buildElementId(this.$resources.enabledClass, downlineRoleId, betType);
            let disabledSpanId = this.buildElementId(this.$resources.disabledClass, downlineRoleId, betType);

            let enabledElement = this.$(enabledSpanId);
            let disabledElement = this.$(disabledSpanId);

            if (enable && this.getTotalStatus(enabledElement) > 0) {
                this.enableElement(enabledElement);
                this.enableElement(disabledElement);
            } else {
                this.disableElement(enabledElement);
                this.disableElement(disabledElement);
            }

            let enableDownline = enable ? (this.parseInt(enabledElement.text()) > 0) : false;
            this.toggleStatusDownlines(downlineRoleId - 1, enableDownline, betType);
        }
    }

    disableElement = (element: any) => element.addClass(this.$resources.grayClass);
    enableElement = (element: any) => element.removeClass(this.$resources.grayClass);
    allowActionOnElement = (element: any) => !element.hasClass(this.$resources.grayClass);

    isEnabledElement = (className: string) => className === this.$resources.enabledClass;

    getRoleId = (element: any) => element.data("roleid");
    getBetType = (element: any) => element.data("bettype");
    getSportType = (element: any) => element.data("sporttype");
    getTotalStatus = (element: any) => element.data("total");
    isChangeState = (originValue: number, currentValue: number) => originValue != currentValue;

    buildElementId = (className: string, roleId: number, betType: number) =>
        ["#", className, "_", roleId, "_", betType].join("");

    parseInt = (value: string) => parseInt(value.replace(/,/g, ""));

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
            let checkboxStatus = self.$(this).find('input:checkbox')
            let bettype = self.getBetType(checkboxStatus);
            let sporttype = self.getSportType(checkboxStatus);
            let disable = !self.$(checkboxStatus).prop("checked");
            let originstatus = self.$.parseJSON(checkboxStatus.data("originstatus").toLowerCase());

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
                || data.updateAllMembers)
            {
                return data;
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

        switch (this.getRoleId(this.$(disabledSpan))) {
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
        this.$.unblockUI();
        this.showMessage(data.Code, data.Message);
        setTimeout(function () {
            location.reload(true);
        }, this.$timeoutDefault);
    }

    onError = () => {
        this.showMessage(this.$errorCodeDefault, this.$resources.updateStatusFailed);
        this.$.unblockUI();
    }

    showMessage = (type: any, message: string) => {
        var html = ['<div class="', type === 0 ? "alertsuccess" : "alerterror", '"><div>', message, "</div></div>"].join("");
        var messageDiv = this.$("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }

    hideMessage = () => this.$("#sectionMessage").html("").addClass("hide");
}

var productStatusSetting = new ProductStatusSetting($);

$(function () {
    productStatusSetting.init();
});