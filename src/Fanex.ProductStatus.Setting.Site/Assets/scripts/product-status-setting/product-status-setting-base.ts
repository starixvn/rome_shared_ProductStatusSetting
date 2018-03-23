class ProductStatusSettingBase  {
    $: JQueryStatic;
    $thead: JQuery;
    $tbody: JQuery;

    $resources: any;
    $errorCodeDefault: number;
    $timeoutDefault: number;

    $custId: number;
    $custRole: number;
    $productId: number;
    $isMultipleBetTypes: boolean;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    protected init() {
        this.cachedElements();

        this.$custId = this.$('#custId').val();
        this.$custRole = this.$('#custRole').val();
        this.$productId = this.$('#productId').val();
        this.$isMultipleBetTypes = this.parseBoolean(this.$("#isMultipleBetTypes").val());

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

    getElements = (elements: HTMLElement[], roleId: number): HTMLElement[] =>
        elements.filter((element, index) => {
            if (this.getRoleId(element) == roleId) {
                return element;
            }
        });

    getFirstElement = (elements: HTMLElement[], roleId: number): HTMLElement => {
        let arrayElement = this.getElements(elements, roleId);
        return arrayElement.length > 0 ? arrayElement[0] : null;
    }

    protected getUpdateViewModel() {
        return {
            productId: this.$productId,
            custId: this.$custId,
            custIds: [],
            custNames: [],
            productStatus: []
        }
    };

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

    callAjax = (updateViewModel: any, actionPost: string) =>
        this.$.ajax({
            type: "POST",
            url: actionPost,
            cache: false,
            headers: this.setupAntiXSRF(),
            data: JSON.stringify(updateViewModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: (data) => this.onSuccess(data),
            error: this.onDefaultError
        });

    protected onSuccess(data: any) {
        this.showMessage(data.Code, data.Message);
        if (data.Code == 0) {
            setTimeout(function () {
                this.$.unblockUI();
                location.reload(true);
            }, this.$timeoutDefault);
        }
    }

    onDefaultError = () => this.showMessage(this.$errorCodeDefault, this.$resources.updateStatusFailed);

    protected showMessage(type: any, message: string) {
        this.$.unblockUI();
        let html = ['<div class="', type === 0 ? "alertsuccess" : "alerterror", '"><div>', message, "</div></div>"].join("");
        let messageDiv = this.$("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }

    hideMessage = () => this.$("#sectionMessage").html("").addClass("hide");
}
