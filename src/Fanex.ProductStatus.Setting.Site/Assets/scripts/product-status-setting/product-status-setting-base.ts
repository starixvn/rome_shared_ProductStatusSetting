class ProductStatusSettingBase implements IProductStatusSettingBase {
    $: JQueryStatic;
    $resources: any;
    $errorCodeDefault: number;
    $timeoutDefault: number;
    $thead: JQuery;
    $tbody: JQuery;

    $custId: number;
    $custRole: number;
    $productId: number;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    protected init () {
        this.cachedElements();

        this.$custId = this.$('#custId').val();
        this.$custRole = this.$('#custRole').val();
        this.$productId = this.$('#productId').val();

        this.$errorCodeDefault = 101;
        this.$timeoutDefault = 3000;
    }

    protected cachedElements() {
        let table = this.$("#tbl-product");
        this.$thead = table.find("thead");
        this.$tbody = table.find("tbody");

        this.$resources = {
            updateStatusFailed: this.$('#updateStatusFailed').val(),
            thereIsNoChange: this.$('#thereIsNoChange').val(),
            enabledClass: "enabled",
            disabledClass: "disabled",
            grayClass: "gray"
        }
    }

    disableElement(element: HTMLElement) {
        this.$(element).addClass(this.$resources.grayClass);
    }

    enableElement (element: HTMLElement) {
        this.$(element).removeClass(this.$resources.grayClass);
    }

    getNextLabel = (element: HTMLElement): HTMLElement =>
        this.$("label[for='" + element.id + "']").toArray()[0] as HTMLElement;

    allowActionOnElement = (element: HTMLElement): boolean =>
        !this.$(element).hasClass(this.$resources.grayClass);

    isEnabledElement = (statusSpan: HTMLElement): boolean =>
        this.$(statusSpan).attr("class") === this.$resources.enabledClass;

    getRoleId = (element: HTMLElement): number =>
        this.$(element).data("roleid");

    isEnablebUpline = (element: HTMLElement): boolean =>
        !this.parseBoolean(this.$(element).data("disabledupline"));

    getBetType = (element: HTMLElement): number =>
        this.$(element).data("bettype");

    getSportType = (element: HTMLElement): number =>
        this.$(element).data("sporttype");

    getTotalStatus = (element: HTMLElement): number =>
        this.$(element).data("total");

    isChecked = (element: HTMLElement): boolean =>
        this.$(element).is(":checked");

    isChangeState = (originValue: number, currentValue: number): boolean =>
        originValue != currentValue;

    buildElementId = (className: string, roleId: number, betType: number): string =>
        ["#", className, "_", roleId, "_", betType].join("");

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

    callAjax = (headers: any, updateViewModel: any, action: string)  =>
        this.$.ajax({
            type: "POST",
            url: action,
            cache: false,
            headers: headers,
            data: JSON.stringify(updateViewModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: this.onSuccess,
            error: this.onError
        })

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
