// Initially copied from glimpse.timeline.js

// glimpse.nlog.js
(function($, pubsub, settings, util, renderEngine, glimpseData) {
    var nlog = {};
    window.nlog = nlog;

    // Elements
    nlog.elements = (function() {
        var elements = {},
            find = function() {
                //Main elements
                elements.scope = nlog.scope;
                elements.dataTable = nlog.scope.find('.glimpse-row-holder');
                elements.dataRows = elements.dataTable.find('tbody tr.glimpse-row');
                elements.filtersTable = nlog.scope.find('.nlog-filters');
                elements.minLevelSelect = nlog.scope.find('select.nlog-minlevel');
                elements.filterInput = nlog.scope.find('input.nlog-filter');
            };

        pubsub.subscribe('action.nlog.shell.loaded', find);

        return elements;
    })();

    // Render Shell
    (function() {
        var setup = function () {

            pubsub.publish('action.nlog.shell.loading');
            
            var htmlUrl = glimpseData.currentMetadata().resources.glimpse_nlog_resource_htmlresource;
            $.get(htmlUrl, function (html) {
                html = html.replace('{0}', nlog.scope.html());
                nlog.scope.html(html);
                pubsub.publish('action.nlog.shell.loaded');
            });
        };

        pubsub.subscribe('trigger.nlog.shell.init', setup);
    })();
    
    // Events
    (function() {
        var events = function () {
            var minLevel = 0, terms = '';
            nlog.elements.dataRows.each(function(i) {
                $(this).data('row-data', glimpseData.currentData().data.glimpse_nlog.data[i + 1][5]);
            });

            var filterChanged = function () {

                nlog.elements.dataRows.each(function() {
                    var data = $(this).data('row-data');

                    var visible = data.levelNumber >= minLevel &&
                        (terms === '' || data.message.toLowerCase().match(terms.toLowerCase()) != null);

                    $(this).toggle(visible);
                });
            };

            filterChanged();

            nlog.elements.minLevelSelect.change(function() {
                minLevel = $(this).val();
                filterChanged();
            });

            nlog.elements.filterInput.on('input change keyup keydown', function () {
                var newTerms = $(nlog.elements.filterInput).val();
                if (newTerms !== terms) {
                    terms = newTerms;
                    filterChanged();
                }
            });
        };

        pubsub.subscribe('action.nlog.shell.loaded', events);
    })();

    // nlog
    (function() {
        var init = function() {
            pubsub.publish('trigger.nlog.shell.init');
        },
            postrender = function(args) {
                nlog.scope = args.panel;
                pubsub.publishAsync('trigger.nlog.init');
            };

        pubsub.subscribe('trigger.nlog.init', init);
        pubsub.subscribe('action.panel.rendered.glimpse_nlog', postrender);
    })();
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.util, glimpse.render.engine, glimpse.data);