// glimpse.nlog.js
(function($, pubsub, settings, util, renderEngine) {
    var nlog = {};
    window.nlog = nlog;

    // Elements
    nlog.elements = (function() {
        var elements = {},
            find = function() {
                //Main elements
                elements.scope = nlog.scope;
                elements.dataTable = nlog.scope.find('.glimpse-row-holder');
                elements.dataRows = elements.dataTable.find('tbody tr');
                elements.filtersTable = nlog.scope.find('.nlog-filters');
                elements.minLevelSelect = nlog.scope.find('.nlog-minlevel');
            };

        pubsub.subscribe('action.nlog.shell.loaded', find);

        return elements;
    })();

    // Render Shell
    (function() {
        var setup = function() {
            pubsub.publish('action.nlog.shell.loading');

            nlog.scope.prepend("<table class='nlog-filters'><thead class='glimpse-row-header glimpse-row-header-0'></thead><tbody>");
            nlog.scope.append("</tbody></table>");

            nlog.scope.find('.nlog-filters thead').html("<tr><th>Min Level: <select class='nlog-minlevel'><option value='1'>Trace</option><option value='2'>Debug</option><option value='3'>Info</option><option value='4'>Warn</option><option value='5'>Error</option><option value='6'>Fatal</option></select></th></tr>");

            pubsub.publish('action.nlog.shell.loaded');
        };

        pubsub.subscribe('trigger.nlog.shell.init', setup);
    })();

    // Events
    (function() {
        var events = function() {
            nlog.elements.minLevelSelect.change(function() {
                var minLevel = $(this).val();
                nlog.elements.dataRows.show()
                    .filter(function() {
                        return $(this).find('td span').first().attr('data-levelNum') < minLevel;
                    }).hide();
            }).change();
        };

        pubsub.subscribe('trigger.nlog.shell.subscriptions', events);
    })();

    // nlog
    (function() {
        var init = function() {
            pubsub.publish('trigger.nlog.shell.init');
            pubsub.publish('trigger.nlog.shell.subscriptions');
        },
            postrender = function(args) {
                nlog.scope = args.panel;
                pubsub.publishAsync('trigger.nlog.init');
            };

        pubsub.subscribe('trigger.nlog.init', init);
        pubsub.subscribe('action.panel.rendered.glimpse_nlog', postrender);
    })();
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.util, glimpse.render.engine);