export default class SupportedCulture {
    id?: string;
    name: string;
    displayName: string;
    dateCreated: Date;
    dateUpdated?: Date;

    constructor() {
        this.id = null;
        this.name = "";
        this.displayName = "";
        this.dateCreated = new Date();
        this.dateUpdated = null;
    }

    static generateMockItem(): SupportedCulture {
        return {
            id: "",
            name: "",
            displayName: "",
            dateCreated: new Date(),
            dateUpdated: null,
        };
    }
}
