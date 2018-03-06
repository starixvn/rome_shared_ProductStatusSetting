class ProductStatusSetting { 
    $: JQueryStatic;
    $enabledSpans: JQuery;
    $disabledSpans: JQuery;
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
        this.$enabledSpans = tbody.find("span.enabled");   
        this.$disabledSpans = tbody.find("span.disabled");
        this.$theadEnableSpan = thead.find("span.enabled");
        this.$theadDisabledSpan = thead.find("span.disabled");

        this.$resources = {
            enabled: this.$('#enabled').val(),
            disabled: this.$('#disabled').val(),
            rootUrl: this.$('#rootUrl').val(),
            updateStatusSuccessfully: this.$('#UpdateStatusSuccessfully').val(),
            updateStatusFailed: this.$('#updateStatusFailed').val(),
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
        self.$enabledSpans
            .unbind("click")
            .bind("click", function () {
                self.toggleStatusBySpan(self.$(this));
            });

    bindingDisableSpanEvent = (self: any) => 
        self.$disabledSpans
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

    checkboxChanged = (element: any) => {
        let labelText = this.$(element).is(":checked") ? this.$resources.enabled : this.$resources.disabled;

        let downlineRoleId = this.getRoleId(element) - 1;
        let betType = this.getBetType(element);
        let enable = this.$(element).is(":checked");
        this.toggleStatusDownlines(downlineRoleId, enable, betType);

        element.next("label")
            .removeClass()
            .addClass(labelText.toLowerCase())
            .text(labelText);
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0)
        {
            let enabledSpanId = this.buildElementId(this.$resources.enabledClass, downlineRoleId, betType);
            let disabledSpanId = this.buildElementId(this.$resources.disabledClass, downlineRoleId, betType);

            let enableStatusSpan = this.$(enabledSpanId);
            let disableStatusSpan = this.$(disabledSpanId);

            if (enable && this.getStatusTotal(enableStatusSpan) > 0) {
                this.enableElement(enableStatusSpan);
                this.enableElement(disableStatusSpan);
            } else {
                this.disableElement(enableStatusSpan);
                this.disableElement(disableStatusSpan);
            }

            let enableDownline = enable ? this.parseInt(enableStatusSpan.text()) > 0 : false;
            this.toggleStatusDownlines(downlineRoleId - 1, enableDownline, betType);
        }
    }

    disableElement = (element: any) => element.addClass(this.$resources.grayClass);
    enableElement = (element: any) => element.removeClass(this.$resources.grayClass);
    isDisableElement = (element: any) => element.hasClass(this.$resources.grayClass);

    toggleStatusBySpan = (statusSpan: any) => this.toggleStatus(statusSpan, statusSpan.attr("class"));

    toggleStatusByHiddenCheck = (statusSpan: any, enableCheckAll: boolean) => 
        this.toggleStatus(statusSpan, enableCheckAll ? this.$resources.enabledClass : this.$resources.disabledClass);

    toggleStatus = (statusSpan: any, className: string) => {
        if (!this.isDisableElement(statusSpan)) {
            let roleId = this.getRoleId(statusSpan);
            let betType = this.getBetType(statusSpan);

            let oppositeClassName = className === this.$resources.enabledClass ? this.$resources.disabledClass : this.$resources.enabledClass;
            let oppositeId = this.buildElementId(oppositeClassName, roleId, betType);
            let oppositeStatus = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatus.text());

            if (oppositeTotal > 0) {
                oppositeStatus.text("0");
                statusSpan.text(this.getStatusTotal(statusSpan));
            }

            this.toggleStatusDownlines(roleId - 1, className === this.$resources.enabledClass, betType);
        }
    }

    toggleAllStatus = (element: any, enable: boolean) => {
        if (!this.isDisableElement(element)) {
            let roleId = this.getRoleId(element);
            this.$custRole == roleId ? this.triggerCheckboxHidden(enable) : this.triggerSpanStatus(roleId, enable);
        }
    }

    triggerSpanStatus = (roleId: number, enable: boolean) => {
        let self = this;
        let statusSpanArray = enable ? this.$enabledSpans : this.$disabledSpans;

        statusSpanArray.each(function (index, element) {
            let statusSpan = self.$(element);
            if (self.getRoleId(statusSpan) === roleId) {
                self.toggleStatusByHiddenCheck(statusSpan, enable);
            }
        });
    }

    triggerCheckboxHidden = (changeAllStatus: boolean) => {
        let self = this;
        this.$checkboxHidden.each(function (index, element) {
            let chkElement = self.$(element);
            if (chkElement.prop("checked") != changeAllStatus) {
                chkElement.click();
            }
        });
    }

    getRoleId = (element: any) => element.data("roleid");
    getBetType = (element: any) => element.data("bettype");
    getSportType = (element: any) => element.data("bettype");
    getStatusTotal = (element: any) => element.data("total");

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
        this.$.ajax({
            type: "POST",
            url: "Update",
            cache: false,
            headers: headers,
            data: JSON.stringify(this.getUpdateViewModelBase()),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: this.onSuccess,
            error: this.onError
        });
    }

    getUpdateViewModelBase = () => {
        let self = this;

        let productStatus = self.$('#tbl-product > tbody  > tr').map(function () {
            let checkboxStatus = self.$(this).find('input:checkbox')
            let bettype = self.getBetType(checkboxStatus);
            let sporttype = self.getSportType(checkboxStatus);
            let disable = !self.$(checkboxStatus).prop("checked");

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

           return data;
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
        let updateAll = !this.isDisableElement(disabledSpan) && totaldisabled != originalTotaldisabled;

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

    hideMessage = () => {
        var messageDiv = this.$("#sectionMessage");
        return messageDiv.html("").addClass("hide");
    }
}

var productStatusSetting = new ProductStatusSetting($);

$(function () {
    productStatusSetting.init();
});