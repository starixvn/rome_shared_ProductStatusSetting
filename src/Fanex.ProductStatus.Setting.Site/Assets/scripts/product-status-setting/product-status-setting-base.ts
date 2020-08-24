class ProductStatusSettingBase {
    $: JQueryStatic;
    $topWindow: any;
    $thead: JQuery;
    $tbody: JQuery;

    $resources: any;
    $errorCodeDefault: number;
    $timeoutDefault: number;

    $custId: number;
    $custName: string;
    $custRole: number;
    $productId: number;
    $leftPanelFrame: string;
    $popupId: string;
    $isMultipleBetTypes: boolean;
    $isReloadLeftPanel: boolean;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.$topWindow = topWindow;
    }

    protected init() {
        this.cachedElements();

        this.$custId = this.$('#custId').val();
        this.$custName = this.$('#custName').val();
        this.$custRole = this.$('#custRole').val();
        this.$productId = this.$('#productId').val();
        this.$isMultipleBetTypes = this.parseBoolean(this.$("#isMultipleBetTypes").val());

        this.$errorCodeDefault = 101;
        this.$timeoutDefault = 3000;
        this.$isReloadLeftPanel = false;
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
            forbiddenClass: "gray"
        }
    }

    bindingSubmitEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    protected disableElement(labelElement: HTMLElement) {
        this.$(labelElement).addClass(this.$resources.forbiddenClass);
    }

    protected enableElement(labelElement: HTMLElement) {
        this.$(labelElement).removeClass(this.$resources.forbiddenClass);
    }

    protected allowActionOnElement(labelElement: HTMLElement): boolean {
        return !this.$(labelElement).hasClass(this.$resources.forbiddenClass);
    }

    protected isEnabledElement(labelElement: HTMLElement): boolean {
        return this.$(labelElement).hasClass(this.$resources.enabledClass);
    }

    getNextLabel = (checkboxElement: HTMLElement): HTMLElement =>
        this.$("label[for='" + checkboxElement.id + "']").toArray()[0] as HTMLElement;

    getRoleId = (element: HTMLElement): number => this.$(element).data("roleid");

    isEnablebUpline = (element: HTMLElement): boolean => !this.parseBoolean(this.$(element).data("disabledupline"));

    getBetType = (element: HTMLElement): number => this.$(element).data("bettype");

    getSportType = (element: HTMLElement): number => this.$(element).data("sporttype");

    isChecked = (element: HTMLElement): boolean => this.$(element).is(":checked");

    isChangeState = (originValue: number, currentValue: number): boolean =>
        originValue != currentValue;

    buildElementId = (className: string, roleId: number, betType: number): string =>
        ["#", className, "_", roleId, "_", betType].join("");

    parseInt = (value: string): number => parseInt(value.replace(/,/g, ""));

    parseBoolean = (value: string): boolean => this.$.parseJSON(value.toLowerCase());

    getTooltip = (element: HTMLElement, enable: boolean): string =>
        enable ? this.$(element).data("disabletooltip") : this.$(element).data("enabletooltip");

    getToggles = (toggles: HTMLElement[], roleId: number): HTMLElement[] =>
        toggles.filter((toggle) => {
            if (this.getRoleId(toggle) == roleId) {
                return toggle;
            }
        });

    getFirstToggle = (toggles: HTMLElement[], roleId: number): HTMLElement =>
        this.getToggles(toggles, roleId)[0];

    protected getUpdateViewModel() {
        return {
            productId: this.$productId,
            custId: this.$custId,
            custRole: this.$custRole,
            custIds: [],
            custNames: [],
            productStatus: []
        }
    }

    setupAntiXSRF() {
        let token = this.$('[name=__RequestVerificationToken]').val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    onSubmit = (e: any) => {
        e.preventDefault();
        let updateViewModel = this.getUpdateViewModel();

        this.$.blockUI({
            blockMsgClass: "block-ui-loading"
        });

        updateViewModel.productStatus.length > 0 ?
            this.callAjax(updateViewModel, this.$resources.updateActionName) :
            this.showMessage(this.$errorCodeDefault, this.$resources.thereIsNoChange);
    }

    callAjax = (updateViewModel: any, postAction: string) =>
        this.$.ajax({
            type: "POST",
            url: postAction,
            cache: false,
            headers: this.setupAntiXSRF(),
            data: JSON.stringify(updateViewModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: (data) => this.onSuccess(data),
            error: this.onDefaultError
        });

    protected onSuccess(data: any) {
        let html = [];
        let isSuccess = true;
        this.$.each(data, function (index, result) {
            let className;

            if (result.Code !== 0) {
                className = "alerterror";
                isSuccess = false;
            } else {
                className = "alertsuccess";
            }

            html.push('<div class="');
            html.push(className);
            html.push('"><div>');
            html.push(result.Message);
            html.push("</div></div>");
        });

        this.showMixedMessages(html.join(""));

        if (isSuccess && this.$topWindow.reloadLeftPanel) {
            setTimeout(() => this.$topWindow.reloadLeftPanel(), this.$timeoutDefault);
        }
    }

    onDefaultError = (jqXHR: any) => {
        if (jqXHR.status === 403) {
            const denyObject = this.$.parseJSON(jqXHR.responseText);
            this.showMessage(denyObject.ErrorCode, denyObject.ErrorMessage);
        } else {
            this.showMessage(this.$errorCodeDefault, this.$resources.updateStatusFailed);
        }
    }

    protected showMessage(code: any, message: string) {
        let html = ['<div class="', code === 0 ? "alertsuccess" : "alerterror", '"><div>', message, "</div></div>"].join("");
        let messageDiv = this.$("#sectionMessage");
        messageDiv.html(html).removeClass("hide");
        this.$.unblockUI();
    }

    protected showMixedMessages(html: string) {
        let messageDiv = this.$("#sectionMessage");
        messageDiv.html(html).removeClass("hide");
        this.$.unblockUI();
    }

    hideMessage = () => this.$("#sectionMessage").html("").addClass("hide");
}
