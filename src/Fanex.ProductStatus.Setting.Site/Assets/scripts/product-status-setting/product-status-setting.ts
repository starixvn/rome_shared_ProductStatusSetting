class ProductStatusSetting { 
    $: JQueryStatic;
    $enabledSpans: JQuery;
    $disabledSpans: JQuery;
    $checkboxHidden: JQuery;
    $theadEnableSpan: JQuery;
    $theadDisabledSpan: JQuery;

    $custId: string;
    $productId: string;
    $resources: any;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();

        this.$custId = this.$('#custId').val();
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
            updatePath: 'ProductStatusSetting/Update'
        }

    }

    checkboxChanged = ($this: any) => {
        let labelText = this.$($this).is(":checked") ? this.$resources.enabled : this.$resources.disabled;
        this.toggleStatusDownlines($this);
        $this.next("label")
            .removeClass()
            .addClass(labelText.toLowerCase())
            .text(labelText);
    }

    toggleStatusDownlines = ($this: any, isEnabled?: boolean) => {
        let checked = false;

        if (typeof (isEnabled) === "undefined" || isEnabled == null) {
            checked = this.$($this).is(":checked");
        }
        else {
            checked = isEnabled;
        }

        let roleId = $this.data("roleid");
        let roleId01 = $this.data("roleid");
        let betType = $this.data("bettype");

        if (checked) {
            while (--roleId > 0) {
                if (this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    this.$("#enabled_" + roleId + "_" + betType).removeClass("gray");
                    this.$("#disabled_" + roleId + "_" + betType).removeClass("gray");
                }
            }
        } else {
            while (--roleId > 0) {
                if (this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    this.$("#enabled_" + roleId + "_" + betType).addClass("gray");
                    this.$("#disabled_" + roleId + "_" + betType).addClass("gray");
                }
            }
        }


        if (this.$("input:checked").length == 0) {
            while (--roleId01 > 0) {
                if (this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    this.$("#enabled_" + roleId + "_" + betType).removeClass("gray");
                    this.$("#disabled_" + roleId + "_" + betType).removeClass("gray");
                }
            }
        }
    }

    toggleStatus = (statusSpan: any, isCheckAll?: boolean, enableCheckAll?: boolean) => {
        if (!statusSpan.hasClass("gray")) {
            let roleId = statusSpan.data("roleid");
            let betType = statusSpan.data("bettype");

            let className = '';
            // var oppositeClassName = '';
            if (isCheckAll) {
                className = enableCheckAll ? "enabled" : "disabled";
            }
            else {
                className = statusSpan.attr("class");
            }

            let oppositeClassName = className === "enabled" ? "disabled" : "enabled";
            let oppositeId = ["#", oppositeClassName, "_", roleId, "_", betType].join("");
            let oppositeStatus = this.$(oppositeId);
            let oppositeTotal = parseInt(oppositeStatus.text().replace(/,/g, ""));

            if (oppositeTotal > 0) {
                oppositeStatus.text("0");
                let currentTotal = parseInt(statusSpan.text().replace(/,/g, ""));
                statusSpan.text((currentTotal + oppositeTotal).toLocaleString());
            }

            this.toggleStatusDownlines(statusSpan, className === "enabled");
        }
    }

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

            self.$checkboxHidden.each(function (index, element) {
                let statusSpan = self.$(element);
                if (statusSpan.data("roleid") === roleId && self.$(statusSpan).prop("checked") != status) {
                    statusSpan.click();
                }
            });
        }
    }

    registerEvents = () => {
        let self = this;

        self.$checkboxHidden.each(function (index, el) {
            self.$(el).bind("change", function () {
                self.checkboxChanged(self.$(this));
            });
        });

        self.$enabledSpans.each(function (index, el) {
            self.$(el).bind("click", function () {
                self.toggleStatus(self.$(this));
            });
        });

        self.$disabledSpans.each(function (index, el) {
            self.$(el).bind("click", function () {
                self.toggleStatus(self.$(this));
            });
        });

        self.$theadEnableSpan.each(function (index, el) {
            self.$(el).bind("click", function () {
                self.toggleAllStatus(self.$(this), true);
            });
        });

        self.$theadDisabledSpan.each(function (index, el) {
            self.$(el).bind("click", function () {
                self.toggleAllStatus(self.$(this), false);
            });
        });

        self.$("#btnReset").on("click", function () { location.reload(true); });
        self.$("#btnSubmit").on("click", self.onSubmit);
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
            error: function (xhr, status, error){
                alert('update fails');
            },
            complete: function (jqXHR, textStatus) {
              //  alert('update complete');
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

        let totaldisabled = Number(statusSpan.text());
        let originalTotaldisabled = Number(statusSpan.data("totaldisabled"));

        let updateAll = !statusSpan.hasClass("gray") && totaldisabled != originalTotaldisabled;
        let disableAll = updateAll && totaldisabled > 0;

        let roleId = statusSpan.data("roleid");
        switch (roleId) {
            case 3:
                model.updateAllMasters = updateAll;
                model.disableAllMasters = disableAll;
                break;
            case 2:
                model.updateAllAgents = updateAll;
                model.disableAllAgents = disableAll;
                break;
            case 1:
                model.updateAllMembers = updateAll;
                model.disableAllMembers = disableAll;
                break;
        };

        return model;
    }

    onSuccess = () => {
       
    }

    onComplete = () => {
       // this.$.unblockUI();
    }

    showSuccessMessage = () => {
        this.showMessage(0, this.$resources.updateStatusSuccessfully);
    }

    showMessage = (type: any, message: string) => {
        var html = ['<div class="', type === 0 ? "alertboxscc" : "alertboxerr", '"><div>', message, "</div></div>"].join("");
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