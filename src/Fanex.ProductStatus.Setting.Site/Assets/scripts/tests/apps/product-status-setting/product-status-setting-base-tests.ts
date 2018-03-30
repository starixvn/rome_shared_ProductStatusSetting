describe('ProductStatusSettingBase_', () => {
    const productStatusSettingBase = new ProductStatusSettingBase($, window.top);
    const fixturesPath = '../../../../../Assets/scripts/tests/apps/product-status-setting/fixtures/';

// Setup
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = fixturesPath;
        jasmine.getFixtures().load('numbergame.html');
    });

    describe("parseInt_", () => {
        it("Should return correct value", function () {
            expect(productStatusSettingBase.parseInt("5")).toEqual(5);
        });
    });

    describe("parseBoolean_", () => {
        it("Should return correct value", function () {
            expect(productStatusSettingBase.parseBoolean("True")).toBeTruthy();
            expect(productStatusSettingBase.parseBoolean("true")).toBeTruthy();

            expect(productStatusSettingBase.parseBoolean("False")).toBeFalsy();
            expect(productStatusSettingBase.parseBoolean("false")).toBeFalsy();
        });
    });

    describe("buildElementId_", () => {
        it("Should return correct Element Id", function () {
            //Arrange
            let expectedElementId = "#enabled_3_12";

            // Act
            let elementId = productStatusSettingBase.buildElementId("enabled", 3, 12);

             // Assert
            expect(elementId).toEqual(expectedElementId);
        });
    });

    describe("getBetType_", () => {
        it("Should return correct bettype id", function () {
            //Arrange
            let elementId = "#enabled_3_81";
            let expectedBettypeId = 81;

            // Act
            let actualBettype = productStatusSettingBase.getBetType((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualBettype).toEqual(expectedBettypeId);
        });
    });

    describe("getSportType_", () => {
        it("Should return correct sporttype id", function () {
            //Arrange
            let elementId = "#enabled_3_81";
            let expectedSportId = 161;

            // Act
            let actualSporttype = productStatusSettingBase.getSportType((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualSporttype).toEqual(expectedSportId);
        });
    });

    describe("getRoleId_", () => {
        it("Should return correct role id", function () {
            //Arrange
            let elementId = "#enabled_3_81";
            let expectedRoleId = 3;

            // Act
            let actualRoleId = productStatusSettingBase.getRoleId((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualRoleId).toEqual(expectedRoleId);
        });
    });

    describe("isEnablebUpline_", () => {
        it("Should return correct enabled upline", function () {
            //Arrange
            let elementId = "#enabled_4_81";
            let expectedEnabledUpline = true;

            // Act
            let actualEnabledUpline = productStatusSettingBase.isEnablebUpline((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualEnabledUpline).toEqual(expectedEnabledUpline);
        });
    });

    describe("isChangeState_", () => {
        it("Should return change state of element", function () {
            expect(productStatusSettingBase.isChangeState(5, 7)).toBeTruthy();
            expect(productStatusSettingBase.isChangeState(5, 5)).toBeFalsy();
        });
    });

    describe("isChecked_", () => {
        it("Should return correct checkbox's status", function () {
            //Arrange
            let elementId = "#enabled_4_81";
            let expectedStatus = true;

            // Act
            let actualStatus = productStatusSettingBase.isChecked((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualStatus).toEqual(expectedStatus);
        });
    });

    describe("getNextLabel_", () => {
        it("Should return correct checkbox's status", function () {
            //Arrange
            let elementId = "#enabled_4_81";
            let expectedStatus = '<label for="enabled_4_81" class="enabled" title="Click to disable all Supers" data-enabletooltip="Click to enable all Supers" data-disabletooltip="Click to disable all Supers">Enabled</label>';

            // Act
            let aa = $(elementId);
            let checboxElement = (<HTMLScriptElement[]><any>$(elementId))[0];
            let actualStatus = productStatusSettingBase.getNextLabel(checboxElement).innerText;

            // Assert
            expect(actualStatus).toEqual(expectedStatus);
        });
    });
});