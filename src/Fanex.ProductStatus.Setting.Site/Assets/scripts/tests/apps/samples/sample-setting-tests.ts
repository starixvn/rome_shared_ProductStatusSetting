describe("sample", function () {
    it("Test", function () {
        loadFixtures('myfixture.html');
        $('#my-fixture').myTestedPlugin();
        expect($('#my-fixture')).toBeEmpty();
    })
})