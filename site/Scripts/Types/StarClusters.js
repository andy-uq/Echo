/// <reference path="Vector.ts" />

var StarCluster = (function () {
    function StarCluster(values) {
        if (values == null)
            return;

        this.id = values.id;
        this.name = values.name;
        this.localCoordinates = values.localCoordinates;
    }
    StarCluster.prototype.size = function () {
        return "N/A";
    };
    return StarCluster;
})();
//# sourceMappingURL=StarClusters.js.map
