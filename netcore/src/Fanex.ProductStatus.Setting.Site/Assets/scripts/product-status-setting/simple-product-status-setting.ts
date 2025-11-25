class SimpleProductStatusSetting {
    $: JQueryStatic;
    topWindow: any;
    productId: number;
    customerId: number;
    customerRole: number;
    postUpdateUrl: string;
    resources: any;
    checkboxMains: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    blockUI(): void {
        this.$.blockUI({
            blockMsgClass: "block-ui-loading"
        });
    }

    showMessage(code: number, message: string): void {
        const html: any = [
            '<div class="',
            code ? "alerterror" : "alertsuccess",
            '"><div>',
            message,
            "</div></div>"];
        this.$("#sectionMessage").html(html.join("")).removeClass("hide");
    }

    hideMessage(): void {
        this.$("#sectionMessage").addClass("hide").html("");
    }

    cacheDOMElements(): void {
        this.productId = this.$("#hiddenProductId").val();
        this.customerId = this.$("#hiddenCustId").val();
        this.customerRole = this.$("#hiddenCustRole").val();
        this.postUpdateUrl = this.$("#hiddenRootUrl").val() + "ProductStatusSetting/SimpleUpdate";
        this.resources = {
            noChange: this.$("#hiddenNoChangeMsg").val(),
            updateStatusFailed: this.$("#hiddenUpdateStatusFailedMsg").val(),
        };
    }

    registerEvents(): void {
        const self: any = this;

        $("#icon-copy-all")
            .on("click", function (): void {
                self.iconCopyAllClicked(self.$(this));
            });

        this.checkboxMains = this.$(".checkbox-main")
            .on("change", function (): void {
                self.checkboxMainChanged(self.$(this));
            });

        this.$("#btnSubmit")
            .on("click", function (e: any): void {
                e.preventDefault();
                self.buttonSubmitClicked();
            });
    }

    iconCopyAllClicked(iconCopyAll: JQuery): void {
        const firstCheckboxMain: JQuery = this.$(this.checkboxMains[0]);
        const firstCheckboxAllDownlines: JQuery = this.getCheckboxAllDownlines(firstCheckboxMain);

        const checkedMain: boolean = firstCheckboxMain.is(":checked");
        const hasAllDownlines: boolean = firstCheckboxAllDownlines.length > 0;
        const checkedAllDownlines: boolean = hasAllDownlines && firstCheckboxAllDownlines.is(":checked");

        for (let i: number = 1, max: number = this.checkboxMains.length; i < max; i++) {
            const checkbox: JQuery = this.$(this.checkboxMains[i]);

            if (!checkbox.is(":disabled")) {
                checkbox.prop("checked", checkedMain);

                if (hasAllDownlines) {
                    this.getCheckboxAllDownlines(checkbox).prop("checked", checkedAllDownlines);
                }

                checkbox.trigger("change");
            }
        }
    }

    checkboxMainChanged(checkbox: JQuery): void {
        const checked: boolean = checkbox.is(":checked");

        const checkboxDownlines: JQuery = this.getCheckboxAllDownlines(checkbox);
        const checkboxDownlinesOriginalValue: boolean = checkboxDownlines.data("originalvalue");

        if (!checkboxDownlinesOriginalValue) {
            checkboxDownlines.prop("checked", checked);
        } else {
            checkboxDownlines.prop("disabled", !checked);
            if (!checked) {
                checkboxDownlines.prop("checked", checked);
            }
        }
    }

    getCheckboxAllDownlines(checkboxMain: JQuery): JQuery {
        return this.$("#checkbox-downlines-"
            + checkboxMain.data("sporttype")
            + "-"
            + checkboxMain.data("bettype"));
    }

    buttonSubmitClicked(): void {
        this.blockUI();
        this.hideMessage();
        const productStatus: any = this.getProductStatusFormData();

        if (productStatus.length) {
            this.updateProductStatus(productStatus);
        } else {
            this.showMessage(1, this.resources.noChange);
            this.$.unblockUI();
        }
    }

    getProductStatusFormData(): any {
        const self: any = this;
        const productStatus: any = [];

        this.$(".checkbox-main").each(function (): void {
            const checkbox: JQuery = self.$(this);
            const disable: boolean = !checkbox.is(":checked");
            const checkboxOriginalValue: boolean = checkbox.data("originalvalue");

            const sportType: number = checkbox.data("sporttype");
            const betType: number = checkbox.data("bettype");
            const checkboxDownlines: JQuery = self.$("#checkbox-downlines-" + sportType + "-" + betType);
            let disableDownlines: boolean = undefined,
                checkboxDownlinesOriginalValue: boolean = undefined;

            if (checkboxDownlines.length) {
                disableDownlines = !checkboxDownlines.is(":checked");
                checkboxDownlinesOriginalValue = checkboxDownlines.data("originalvalue");
            }

            if (disable !== checkboxOriginalValue || disableDownlines !== checkboxDownlinesOriginalValue) {
                productStatus.push({
                    sportType: sportType,
                    betType: betType,
                    disable: disable,
                    disableDownlines: disableDownlines !== checkboxDownlinesOriginalValue ? disableDownlines : undefined
                });
            }
        });

        return productStatus;
    }

    setupAntiXSRF(): any {
        const token: string = this.$("[name=__RequestVerificationToken]").val();
        const headers: any = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    updateProductStatus(productStatus: any): void {
        const formData: any = {
            productId: this.productId,
            custId: this.customerId,
            custRole: this.customerRole,
            productStatus: productStatus
        };

        const jqxhr: any = this.$.ajax({
            type: "POST",
            url: this.postUpdateUrl,
            cache: false,
            headers: this.setupAntiXSRF(),
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });

        jqxhr
            .done((data: any) => {
                this.showMessage(data.Code, data.Message);

                if (!data.Code) {
                    setTimeout(this.topWindow.reloadLeftPanel, 1000 * 3);
                }
            })
            .fail(() => { this.showMessage(1, this.resources.updateStatusFailed); })
            .always(this.$.unblockUI);
    }

    initialize(): void {
        this.cacheDOMElements();
        this.registerEvents();
    }
}

$(function (): void {
    new SimpleProductStatusSetting($, window.top).initialize();
});