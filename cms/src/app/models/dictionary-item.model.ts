export default class DictionaryItem {
    id?: string;
    cultureName: string;
    key: string;
    name: string;
    value: string;
    dateCreated: Date;
    dateUpdated?: Date;

    constructor() {
        this.id = null;
        this.cultureName = "";
        this.key = "";
        this.value = "";
        this.dateCreated = new Date();
        this.dateUpdated = null;
    }

    static generateMockItem(): DictionaryItem {
        return {
            id: "",
            cultureName: "",
            key: "",
            name: "",
            value: "",
            dateCreated: new Date(),
            dateUpdated: null,
        };
    }
}
