describe('ProductStatusSetting_', () => {
    const productStatusBaseSetting = new ProductStatusSettingBase($);
    const fixturesPath = '../../../../../Assets/scripts/tests/apps/product-status-setting/fixtures/';

// Setup
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = fixturesPath;
        jasmine.getFixtures().load('numbergame.html');
    });

    describe("parseInt_", () => {
        it("Should return correct value", function () {
            expect(productStatusBaseSetting.parseInt("5")).toEqual(5);
        });
    });

    describe("parseBoolean_", () => {
        it("Should return correct value", function () {
            expect(productStatusBaseSetting.parseBoolean("True")).toBeTruthy();
            expect(productStatusBaseSetting.parseBoolean("true")).toBeTruthy();

            expect(productStatusBaseSetting.parseBoolean("False")).toBeFalsy();
            expect(productStatusBaseSetting.parseBoolean("false")).toBeFalsy();
        });
    });

    describe("buildElementId_", () => {
        it("Should return correct Element Id", function () {
            // Act
            let elementId = productStatusBaseSetting.buildElementId("enabled", 3, 12);

             // Assert
            expect(elementId).toEqual("#enabled_3_12");
        });
    });

    describe("getBetType_", () => {
        it("Should return correct bettype id", function () {
            // Act
            let elementId = productStatusBaseSetting.buildElementId("enabled", 3, 81);
            let actualSporttype = productStatusBaseSetting.getBetType((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualSporttype).toEqual(81);
        });
    });

    describe("getSportType_", () => {
        it("Should return correct sporttype id", function () {
            // Act
            let elementId = productStatusBaseSetting.buildElementId("enabled", 3, 81);
            let actualBettype = productStatusBaseSetting.getSportType((<HTMLScriptElement[]><any>$(elementId))[0]);

            // Assert
            expect(actualBettype).toEqual(161);
        });
    });

    describe("isChangeState_", () => {
        it("Should return change state of element", function () {
            expect(productStatusBaseSetting.isChangeState(5, 7)).toBeTruthy();
            expect(productStatusBaseSetting.isChangeState(5, 5)).toBeFalsy();
        });
    });

});