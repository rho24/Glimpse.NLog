// Initially copied from glimpse.timeline.js

// glimpse.nlog.js
(function($, pubsub, settings, util, renderEngine, data) {
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
        var setup = function () {

            pubsub.publish('action.nlog.shell.loading');
            
            var htmlUrl = data.currentMetadata().resources.glimpse_nlog_resource_htmlresource;
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
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.util, glimpse.render.engine, glimpse.data);