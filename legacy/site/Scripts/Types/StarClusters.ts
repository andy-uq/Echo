/// <reference path="Vector.ts" />

interface IStarCluster {
    id: string;
    name: string;
    localCoordinates: Vector;
}

class StarCluster implements IStarCluster {
    id: string;
    name: string;
    localCoordinates: Vector;

    constructor(values: IStarCluster) {
        if (values == null)
            return;

        this.id = values.id;
        this.name = values.name;
        this.localCoordinates = values.localCoordinates;
    }

    size() : string {
        return "N/A";
    }
}



