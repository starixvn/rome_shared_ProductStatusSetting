class ProductStatusSetting { 
    $: JQueryStatic;
    $enabledSpans: JQuery;
    $disabledSpans: JQuery;
    $checkboxHidden: JQuery;
    $theadEnableSpan: JQuery;
    $theadDisabledSpan: JQuery;

    $custId: string;
    $productId: string;

    resources = {
        enabled: (<HTMLInputElement>document.getElementById('enabled')).value,
        disabled: (<HTMLInputElement>document.getElementById('disabled')).value,
        rootUrl: (<HTMLInputElement>document.getElementById('rootUrl')).value,
        updatePath: 'ProductStatusSetting/Update'
    }

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cachedElements();
        this.registerEvents();
    }

    cachedElements = () => {
        let self = this;
        let table = self.$("#tbl-product");
        let thead = table.find("thead");
        let tbody = table.find("tbody");

        this.$checkboxHidden = self.$("input[class='checkbox-hide']");
        this.$enabledSpans = tbody.find("span.enabled");   
        this.$disabledSpans = tbody.find("span.disabled");
        this.$theadEnableSpan = thead.find("span.enabled");
        this.$theadDisabledSpan = thead.find("span.disabled");

        this.$custId = (<HTMLInputElement>document.getElementById('custId')).value;
        this.$productId = (<HTMLInputElement>document.getElementById('productId')).value;
    }

    checkboxChanged = ($this: any) => {
        let labelText = this.$($this).is(":checked") ? this.resources.enabled : this.resources.disabled;
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

        this.$.blockUI();

        let updateViewModelBase =
            { ProductId: 5, ProductStatus: [] };
        this.$.ajax({
            type: "POST",
            url: this.resources.rootUrl + this.resources.updatePath,
            data: updateViewModelBase,
            dataType: "json",
            success: this.onSuccess,
            complete: this.onComplete
        });
    }

    onSuccess = () => {
        this.$.unblockUI();
    }

    onComplete = () => {
        this.$.unblockUI();
    }
}

var productStatusSetting = new ProductStatusSetting($);

$(function () {
    productStatusSetting.init();
});