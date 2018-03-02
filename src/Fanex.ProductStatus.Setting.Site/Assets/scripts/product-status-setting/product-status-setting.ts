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

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();

        this.$custId = this.$('#custId').val();
        this.$custRole = this.$('#custRole').val();
        this.$productId = this.$('#productId').val();
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
            updateStatusFailed: this.$('#updateStatusFailed').val()
        }
    }

    registerEvents = () => {
        let self = this;

        self.$checkboxHidden.unbind("change")
            .bind("change", function () {
                self.checkboxChanged(self.$(this));
            });

        self.$enabledSpans
            .unbind("click")
            .bind("click", function () {
                self.toggleStatus(self.$(this));
            });

        self.$disabledSpans
            .unbind("click")
            .bind("click", function () {
                self.toggleStatus(self.$(this));
            });

        self.$theadEnableSpan
            .unbind("click")
            .bind("click", function () {
                self.toggleAllStatus(self.$(this), true);
            });

        self.$theadDisabledSpan.unbind("click")
            .bind("click", function () {
                self.toggleAllStatus(self.$(this), false);
            });

        self.$("#btnReset").on("click", function () { location.reload(true); });
        self.$("#btnSubmit").on("click", self.onSubmit);
    }

    checkboxChanged = ($this: any) => {
        let labelText = this.$($this).is(":checked") ? this.$resources.enabled : this.$resources.disabled;

        let downlineRoleId = $this.data("roleid") - 1;
        let betType = $this.data("bettype");
        let enable = this.$($this).is(":checked");
        this.toggleStatusDownlines(downlineRoleId, enable, betType);

        $this.next("label")
            .removeClass()
            .addClass(labelText.toLowerCase())
            .text(labelText);
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0)
        {
            let enableStatusSpan = this.$("#enabled_" + downlineRoleId + "_" + betType);
            let disableStatusSpan = this.$("#disabled_" + downlineRoleId + "_" + betType);

            if (enable) {
                enableStatusSpan.removeClass("gray");
                disableStatusSpan.removeClass("gray");
                
            } else {
                if (enableStatusSpan.data("total") != 0) {
                    enableStatusSpan.addClass("gray");
                    disableStatusSpan.addClass("gray");
                }
            }

            let levelStatus = this.getLevelStatus(enableStatusSpan);
            this.toggleStatusDownlines(downlineRoleId - 1, levelStatus.DisableAll, betType);
        }
    }

    toggleStatus = (statusSpan: any, isCheckAll?: boolean, enableCheckAll?: boolean) => {
        if (!statusSpan.hasClass("gray")) {
            let roleId = statusSpan.data("roleid");
            let betType = statusSpan.data("bettype");

            let className = '';
            if (isCheckAll) {
                className = enableCheckAll ? "enabled" : "disabled";
            }
            else {
                className = statusSpan.attr("class");
            }

            let oppositeClassName = className === "enabled" ? "disabled" : "enabled";
            let oppositeId = ["#", oppositeClassName, "_", roleId, "_", betType].join("");
            let oppositeStatus = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatus.text());

            if (oppositeTotal > 0) {
                oppositeStatus.text("0");
                let currentTotal = this.parseInt(statusSpan.text());
                statusSpan.text((currentTotal + oppositeTotal).toLocaleString());
            }

            this.toggleStatusDownlines(roleId - 1, className === "enabled", betType);
        }
    }

    parseInt = (value: string) => parseInt(value.replace(/,/g, ""));

    toggleAllStatus = ($this: any, status: boolean) => {
        let self = this;

        if (!$this.hasClass("gray")) {
            let roleId = $this.data("roleid");
            let statusSpanArray = status ? self.$enabledSpans : self.$disabledSpans;

            statusSpanArray.each(function (index, element) {
                let statusSpan = self.$(element);
                if (statusSpan.data("roleid") === roleId) {
                    self.toggleStatus(statusSpan, true, status);
                }
            });

            if (self.$custRole == roleId) {
                self.$checkboxHidden.each(function (index, element) {
                    let statusSpan = self.$(element);
                    if (statusSpan.data("roleid") === roleId && self.$(statusSpan).prop("checked") != status) {
                        statusSpan.click();
                    }
                });
            }
        }
    }

    onSubmit = (e: any) => {
        e.preventDefault();

        //this.$.blockUI();

        let self = this;
        let updateViewModelBase = this.getUpdateViewModelBase();
        this.$.ajax({
            type: "POST",
            url: "Update",
            data: JSON.stringify(updateViewModelBase),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, textStatus, jqXHR) {
                self.showMessage(data.Code, data.Message)
            },
            error: function (xhr, status, error) {
                self.showMessage(101, self.$resources.updateStatusFailed)
            },
            complete: function (jqXHR, textStatus) {
              // this.$.unblockUI();
            }
        });
    }

    getUpdateViewModelBase = () => {
        let self = this;

        let productStatus = self.$('#tbl-product > tbody  > tr').map(function () {

            let checkboxStatus = self.$(this).find('input:checkbox')
            let bettype = checkboxStatus.data("bettype");
            let sporttype = checkboxStatus.data("sporttype");
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

    getproductStatus = (spanElement: any, model: any) =>{
        let statusSpan = this.$(spanElement);
        let levelStatus = this.getLevelStatus(statusSpan);
        let roleId = statusSpan.data("roleid");

        switch (roleId) {
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

    getLevelStatus = (statusSpan: any) => {
        let totaldisabled = this.parseInt(statusSpan.text());
        let originalTotaldisabled = statusSpan.data("totaldisabled");
        let updateAll = !statusSpan.hasClass("gray") && totaldisabled != originalTotaldisabled;

        return {
            UpdateAll: updateAll,
            DisableAll: updateAll && totaldisabled > 0
        }
    }

    onSuccess = () => {
       
    }

    onComplete = () => {
       // this.$.unblockUI();
    }

    //showSuccessMessage = () => {
    //    this.showMessage(0, this.$resources.updateStatusSuccessfully);
    //}

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