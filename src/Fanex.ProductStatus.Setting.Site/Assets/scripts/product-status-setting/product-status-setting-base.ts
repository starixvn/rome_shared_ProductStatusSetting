class ProductStatusSettingBase {
    $: JQueryStatic;
    $resources: any;
    $errorCodeDefault: number;
    $timeoutDefault: number;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    disableElement = (element: HTMLElement) =>
        this.$(element).addClass(this.$resources.grayClass);

    enableElement = (element: HTMLElement) =>
        this.$(element).removeClass(this.$resources.grayClass);

    getNextLable = (element: HTMLElement): JQuery =>
        this.$(element).parent().find("label");

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
