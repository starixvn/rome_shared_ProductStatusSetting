describe('jasmine.Fixtures', function () {
    // Setup
    beforeEach(function () {
        //jasmine.getFixtures().fixturesPath = 'spec/fixtures/';
        //jasmine.getFixtures().load('myfixture.html');
    });

    describe("Sample", function () {    
        it("First test", function () {
            jasmine.getFixtures().fixturesPath = 'spec/fixtures/';
            jasmine.getFixtures().load('myfixture.html');
            expect($('#my-fixture')).toBeEmpty();
        });


        //it("Handles HTML Fixtures", function () {
        //    // jasmine.getFixtures().fixturesPath = 'my/new/path';
        //   // jasmine.getFixtures().containerId = 'my-new-id';

        //    jasmine.getFixtures().load('myfixture.html');
        //    jasmine.getFixtures().appendLoad('myfixture.html', 'myfixture2.html');
        //    jasmine.getFixtures().read('myfixture.html', 'myfixture2.html');
        //    jasmine.getFixtures().set('<html></html>');
        //    jasmine.getFixtures().appendSet('<html></html>');
        //    jasmine.getFixtures().preload('myfixture.html', 'myfixture2.html');
        //    jasmine.getFixtures().clearCache();
        //    jasmine.getFixtures().cleanUp();

        //    loadFixtures('myfixture.html');
        //    appendLoadFixtures('myfixture.html');
        //    readFixtures('myfixture.html');
        //    setFixtures('<html></html>');
        //    appendSetFixtures('<html></html>');

        //    sandbox();
        //    sandbox({
        //        id: 'my-id',
        //        class: 'my-class',
        //        myattr: 'my-attr'
        //    });

        //    setFixtures(sandbox({ class: 'my-class' }));
        //});

    })
});
