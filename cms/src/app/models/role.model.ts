export default class Role {
    id?: string;
    name: string;
    dateCreated: Date;
    dateUpdated?: Date;

    constructor() {
        this.id = "";
        this.name = "";
        this.dateCreated = new Date();
        this.dateUpdated = null;
    }

    static generateMockRole(): Role {
        return {
            id: "",
            name: "",
            dateCreated: new Date(),
            dateUpdated: null,
        };
    }
}
