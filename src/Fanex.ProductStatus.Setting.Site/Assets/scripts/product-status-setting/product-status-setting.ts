class ProductStatusSetting extends ProductStatusSettingBase {
    $: JQueryStatic;
    $tbodyEnabledSpans: HTMLElement[];
    $tbodyDisabledSpans: HTMLElement[];
    $checkboxHidden: HTMLElement[];
    $theadEnableSpan: HTMLElement[];
    $theadDisabledSpan: HTMLElement[];

    constructor($: JQueryStatic) {
        super($);
    }

    init() {
        super.init();
        this.registerEvents();
    }

    cachedElements() {
        super.cachedElements();

        this.$checkboxHidden = this.$("input[class='checkbox-hide']").toArray() as HTMLElement[];
        this.$tbodyEnabledSpans = this.$tbody.find("span.enabled").toArray() as HTMLElement[];
        this.$tbodyDisabledSpans = this.$tbody.find("span.disabled").toArray() as HTMLElement[];
        this.$theadEnableSpan = this.$thead.find("span.enabled").toArray() as HTMLElement[];
        this.$theadDisabledSpan = this.$thead.find("span.disabled").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enabled').val();
        this.$resources.disabled = this.$('#disabled').val();
    }

    bindingCheckboxHiddenEvent = () =>
        this.$checkboxHidden.forEach((element) => {
            element.addEventListener('click', () => {
                this.checkboxChanged(element);
            });
        });


    bindingEnableSpanEvent = () =>
        this.$tbodyEnabledSpans.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleStatus(element);
            });
        });

    bindingDisableSpanEvent = () =>
        this.$tbodyDisabledSpans.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleStatus(element);
            });
        });

    bindingTheadEnableSpanEvent = () =>
        this.$theadEnableSpan.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleAllStatus(element, true);
            });
        });

    bindingTheadDisableSpanEvent = () =>
        this.$theadDisabledSpan.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleAllStatus(element, false);
            });
        });


    bindingSubmitButtonEvent = () => this.$("#btnSubmit").on("click", this.onSubmit);
    bindingResetButtonEvent = () => this.$("#btnReset").on("click", function () { location.reload(true); });

    registerEvents = () => {
        this.bindingCheckboxHiddenEvent();
        this.bindingEnableSpanEvent();
        this.bindingDisableSpanEvent();
        this.bindingTheadEnableSpanEvent();
        this.bindingTheadDisableSpanEvent();

        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toggleAllStatus = (element: HTMLElement, enable: boolean) => {
        if (this.allowActionOnElement(element)) {
            let roleId = this.getRoleId(element);
            this.$custRole == roleId ? this.toggleAllStatusOnCheckbox(enable) : this.toggleAllStatusOnSpans(roleId, enable);
        }
    }

    toggleAllStatusOnSpans = (toggleAllRoleId: number, enable: boolean) => {
        let statusSpanArray = enable ? this.$tbodyEnabledSpans : this.$tbodyDisabledSpans;

        statusSpanArray.forEach((element, index) => {
            if (this.getRoleId(element) === toggleAllRoleId) {
                element.click();
            }
        });
    }

    toggleAllStatusOnCheckbox = (enableAllStatus: boolean) =>
        this.$checkboxHidden.forEach((element, index) => {
            if (this.isChecked(element) != enableAllStatus) {
                element.click();
            }
        });

    checkboxChanged = (element: HTMLElement) => {
        if (this.isEnablebUpline(element)) {
            let enable = this.isChecked(element);
            let labelElement = this.getNextLabel(element);

            let tooltip = enable ? this.$(labelElement).data("disabletooltip") : this.$(labelElement).data("enabletooltip")
            let labelText = enable ? this.$resources.enabled : this.$resources.disabled;

            this.$(labelElement).attr("title", tooltip)
                .removeClass()
                .addClass(labelText.toLowerCase())
                .text(labelText);

            this.toggleStatusDownlines(this.getRoleId(element) - 1, enable, this.getBetType(element));
        }
    }

    toggleStatus = (statusSpan: HTMLElement) => {
        if (this.allowActionOnElement(statusSpan)) {
            let enable = this.isEnabledElement(statusSpan);
            let roleId = this.getRoleId(statusSpan);
            let betType = this.getBetType(statusSpan);

            let oppositeClassName = enable ? this.$resources.disabledClass : this.$resources.enabledClass;
            let oppositeId = this.buildElementId(oppositeClassName, roleId, betType);
            let oppositeStatusSpan = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatusSpan.text());

            if (oppositeTotal > 0) {
                oppositeStatusSpan.text("0");
                this.$(statusSpan).text(this.getTotalStatus(statusSpan));
            }

            this.toggleStatusDownlines(roleId - 1, enable, betType);
        }
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0) {
            let enabledSpan = this.getDownlineElement(this.$tbodyEnabledSpans, downlineRoleId, betType);
            let disabledSpan = this.getDownlineElement(this.$tbodyDisabledSpans, downlineRoleId, betType);

            if (enable && this.getTotalStatus(enabledSpan) > 0) {
                this.enableElement(enabledSpan);
                this.enableElement(disabledSpan);
            } else {
                this.disableElement(enabledSpan);
                this.disableElement(disabledSpan);
            }

            let currentTotalEnabled = this.$(enabledSpan).text();
            let enableDownline = enable ? (this.parseInt(currentTotalEnabled) > 0) : false;
            this.toggleStatusDownlines(downlineRoleId - 1, enableDownline, betType);
        }
    }

    getDownlineElement = (elements: HTMLElement[], roleId: number, bettype: number): HTMLElement => {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId && this.getBetType(elements[index]) == bettype) {
                return elements[index];
            }
        }

        return null;
    }

    onSubmit = (e: any) => {
        e.preventDefault();

        let headers = this.setupAntiXSRF();
        let updateViewModel = this.getUpdateViewModel();

        this.$.blockUI({
            blockMsgClass: "block-ui-loading"
        });

        updateViewModel.productStatus.length > 0 ?
            this.callAjax(headers, updateViewModel, "Update") :
            this.showMessage(this.$errorCodeDefault, this.$resources.thereIsNoChange);
    }

    getUpdateViewModel = () => {
        let self = this;

        let productStatus = self.$('#tbl-product > tbody  > tr').map(function () {
            let checkboxStatus = (self.$(this).find('input:checkbox').toArray() as HTMLElement[])[0];
            if (self.isEnablebUpline(checkboxStatus)) {
                let bettype = self.getBetType(checkboxStatus);
                let sporttype = self.getSportType(checkboxStatus);
                let disable = !self.isChecked(checkboxStatus);
                let originstatus = self.parseBoolean(self.$(checkboxStatus).data("originstatus"));

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
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        return {
            productId: this.$productId,
            custId: this.$custId,
            productStatus: productStatus
        };
    }

    getproductStatus = (disabledSpan: HTMLElement, model: any) => {
        let levelStatus = this.getLevelStatus(this.$(disabledSpan));

        switch (this.getRoleId(disabledSpan)) {
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
        let originalTotaldisabled = this.getTotalStatus(disabledSpan);
        let updateAll = this.allowActionOnElement(disabledSpan) && this.isChangeState(originalTotaldisabled, totaldisabled);

        return {
            UpdateAll: updateAll,
            DisableAll: updateAll && totaldisabled > 0
        }
    }
}

$(function () {
    let productStatusSetting = new ProductStatusSetting($);
    productStatusSetting.init();
});