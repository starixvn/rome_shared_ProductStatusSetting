class ProductStatusSetting extends ProductStatusSettingBase {
    $: JQueryStatic;
    $enableAllArray: HTMLElement[];
    $disableAllArray: HTMLElement[];

    $toggleUplineArray: HTMLElement[];

    $enableDownlineArray: HTMLElement[];
    $disableDownlineArray: HTMLElement[];

    constructor($: JQueryStatic) {
        super($);
    }

    init() {
        super.init();
        this.registerEvents();
    }

    cachedElements() {
        super.cachedElements();

        this.$enableAllArray = this.$thead.find("span.enabled").toArray() as HTMLElement[];
        this.$disableAllArray = this.$thead.find("span.disabled").toArray() as HTMLElement[];

        this.$toggleUplineArray = this.$("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$enableDownlineArray = this.$tbody.find("span.enabled").toArray() as HTMLElement[];
        this.$disableDownlineArray = this.$tbody.find("span.disabled").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enabled').val();
        this.$resources.disabled = this.$('#disabled').val();
        this.$resources.updateActionName = "Update";
    }

    bindingtoggleUplineEvent = () =>
        this.$toggleUplineArray.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleUpline(element);
            });
        });

    bindingEnableDownlineEvent = () =>
        this.$enableDownlineArray.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleDownline(element);
            });
        });

    bindingDisableDownlineEvent = () =>
        this.$disableDownlineArray.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleDownline(element);
            });
        });

    bindingEnableAllEvent = () =>
        this.$enableAllArray.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleAllStatus(element, true);
            });
        });

    bindingDisableAllEvent = () =>
        this.$disableAllArray.forEach((element) => {
            element.addEventListener('click', () => {
                this.toggleAllStatus(element, false);
            });
        });

    registerEvents = () => {
        this.bindingtoggleUplineEvent();
        this.bindingEnableDownlineEvent();
        this.bindingDisableDownlineEvent();
        this.bindingEnableAllEvent();
        this.bindingDisableAllEvent();

        this.bindingResetButtonEvent();
        this.bindingSubmitButtonEvent();
    }

    toggleAllStatus = (element: HTMLElement, enable: boolean) => {
        if (this.allowActionOnElement(element)) {
            let roleId = this.getRoleId(element);
            this.$custRole == roleId ? this.toggleAllStatusUplines(enable) : this.toggleAllStatusDownlines(roleId, enable);
        }
    }

    toggleAllStatusDownlines = (toggleAllRoleId: number, enable: boolean) => {
        let statusSpanArray = enable ? this.$enableDownlineArray : this.$disableDownlineArray;

        statusSpanArray.forEach((element, index) => {
            if (this.getRoleId(element) === toggleAllRoleId) {
                element.click();
            }
        });
    }

    toggleAllStatusUplines= (enableAllStatus: boolean) =>
        this.$toggleUplineArray.forEach((element, index) => {
            if (this.isChecked(element) != enableAllStatus) {
                element.click();
            }
        });

    toggleUpline = (element: HTMLElement) => {
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

    toggleDownline = (statusSpan: HTMLElement) => {
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

    watchHeader = (roleId: number) => {
        if (this.$isMultipleBetTypes) {
            this.toogleHeader(roleId);
        }
    }

    isDisableHeader = (tbodyElements: HTMLElement[]): boolean => 
        tbodyElements.every((element) => {
            return !this.allowActionOnElement(element);
        });

    toogleHeader = (roleId: number) => {
        let tbodyElements = this.getArrayElement(this.$enableDownlineArray, roleId);
        let theadEnabledElement = this.getFirstElement(this.$enableAllArray, roleId);
        let theadDisabledElement = this.getFirstElement(this.$disableAllArray, roleId);

        if (tbodyElements != null && tbodyElements.length > 0 && this.isDisableHeader(tbodyElements)) {
            this.disableElement(theadEnabledElement);
            this.disableElement(theadDisabledElement);
        }
        else {
            this.enableElement(theadEnabledElement);
            this.enableElement(theadDisabledElement);
        }
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0) {
            let enabledSpan = this.getDownlineElement(this.$enableDownlineArray, downlineRoleId, betType);
            let disabledSpan = this.getDownlineElement(this.$disableDownlineArray, downlineRoleId, betType);

            if (enable && this.getTotalStatus(enabledSpan) > 0) {
                this.enableElement(enabledSpan);
                this.enableElement(disabledSpan);
            } else {
                this.disableElement(enabledSpan);
                this.disableElement(disabledSpan);
            }

            this.watchHeader(downlineRoleId);

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

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, element) => {
            let checkboxStatus = (this.$(element).find('input:checkbox').toArray() as HTMLElement[])[0];
            if (this.isEnablebUpline(checkboxStatus)) {
                let bettype = this.getBetType(checkboxStatus);
                let sporttype = this.getSportType(checkboxStatus);
                let disable = !this.isChecked(checkboxStatus);
                let originstatus = this.parseBoolean(this.$(checkboxStatus).data("originstatus"));

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

                this.$(this).find('span:odd').each(function () {
                    return this.getProductStatus(this, data);
                });

                if (data.disable != originstatus
                    || data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        viewModel.productStatus = productStatus;
        return viewModel;
    }

    getProductStatus(disabledSpan: HTMLElement, model: any) {
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

    getLevelStatus(disabledSpan: any) {
        let totaldisabled = this.parseInt(disabledSpan.text());
        let originalTotaldisabled = this.getTotalStatus(disabledSpan);
        let updateAll = this.allowActionOnElement(disabledSpan) && this.isChangeState(originalTotaldisabled, totaldisabled);

        return {
            UpdateAll: updateAll,
            DisableAll: updateAll && totaldisabled > 0
        }
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
}

$(function () {
    let productStatusSetting = new ProductStatusSetting($);
    productStatusSetting.init();
});