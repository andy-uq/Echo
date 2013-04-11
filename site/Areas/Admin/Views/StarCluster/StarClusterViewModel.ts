/// <reference path="../../../../scripts/typedef/knockout.d.ts" />
/// <reference path="../../../../scripts/typedef/knockout-postbox.d.ts" />

/// <reference path="../../../../scripts/Types/Vector.ts" />
/// <reference path="../../../../scripts/Types/StarClusters.ts" />
/// <reference path="../../../../scripts/ViewModels/VectorViewModel.ts" />

class StarClusterViewModel {
    id: string;
    name: KnockoutObservableString;
    localCoordinates: KnockoutObservableAny;
    isEditing: KnockoutObservableString;
    size: KnockoutComputed;

    private previousState: StarCluster;

    constructor(starCluster: StarCluster) {
        this.id = starCluster.id;
        this.name = ko.observable(starCluster.name);
        this.localCoordinates = ko.observable(new VectorViewModel(starCluster.localCoordinates));
        this.isEditing = ko.observable();
        this.size = ko.computed(() => 'N/A');
    }

    save(): StarCluster {
        return new StarCluster({
            id: this.id,
            name: this.name(),
            localCoordinates: this.localCoordinates().save()
        });
    }

    beginRename() {
        if (!this.isEditing()) {
            this.isEditing("name");
            this.previousState = this.save();
        }
    }

    beginEditCoords() {
        if (!this.isEditing()) {
            this.isEditing("coords");
            this.previousState = this.save();
        }
    }

    cancel() {
        this.isEditing(null);
        this.name(this.previousState.name);
        this.localCoordinates(this.previousState.localCoordinates);
    }

    rename() {
        this.isEditing(null);
        var starCluster = this.save();
        ko.postbox.publish("starcluster.rename", starCluster);
    }

    editCoords() {
        this.isEditing(null);
        var starCluster = this.save();
        ko.postbox.publish("starcluster.updatePosition", starCluster);
    }

    add() {
        var starCluster = this.save();
        ko.postbox.publish("starcluster.new", starCluster);
        this.reset();
    }

    reset() {
        this.name("");
        this.localCoordinates().reset()
    }
}