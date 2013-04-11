/// <reference path="../../../../scripts/typedef/knockout.d.ts" />
/// <reference path="../../../../scripts/typedef/knockout-postbox.d.ts" />

/// <reference path="../../../../scripts/Types/Vector.ts" />
/// <reference path="../../../../scripts/Types/StarClusters.ts" />
/// <reference path="StarClusterViewModel.ts" />

declare var starClusters: StarCluster[];

class StarClusterIndexViewModel {
    data: KnockoutObservableArray;
    newStarCluster: KnockoutObservableAny;

    remove(starCluster: StarClusterViewModel) { }
    edit(starCluster: StarClusterViewModel) { }

    constructor(starClusters: StarCluster[]) {
        var self = this;

        this.data = ko.observableArray();
        for (var i in starClusters)
            this.data.push(new StarClusterViewModel(starClusters[i]))

        this.newStarCluster = ko.observable(new StarClusterViewModel(new StarCluster(null)));

        this.remove = x => {
            self.data.remove(x);
        }
        this.edit = x => {
            x.beginRename();
        }

        ko.postbox.subscribe("starcluster.new", x => { self.new (x) });
        ko.postbox.subscribe("starcluster.rename", x => { self.update(x) });
    }

    new(starCluster: StarCluster) {
        starCluster.id = "starClusters/2";
        this.data.push(new StarClusterViewModel(starCluster));
    }

    find(id: string): StarClusterViewModel {
        for (var i = 0; i < this.data().length; i++) {
            var value: StarClusterViewModel = this.data()[i];
            if (value.id == id)
                return value;
        }

        return null;
    }

    update(starCluster: StarCluster) {
        // todo: post update to server
        this.resetEdit();
    }

    resetEdit() {
        this.newStarCluster(new StarClusterViewModel(new StarCluster(null)));
    }
}

declare var $;

// Activates knockout.js
$(function () { ko.applyBindings(new StarClusterIndexViewModel(starClusters)) });