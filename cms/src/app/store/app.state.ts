import { DictionaryState } from "./dictionary/dictionary.state";
import { RoleListState } from "./roles/role.state";

export default interface AppState {
    roles: RoleListState;
    dictionary: DictionaryState;
}
