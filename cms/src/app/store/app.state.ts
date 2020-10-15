import { DictionaryState } from "./dictionary/dictionary.state";
import { RoleListState } from "./roles/role.state";
import { UsersState } from "./users/user.state";

export default interface AppState {
    roles: RoleListState;
    dictionary: DictionaryState;
    users: UsersState;
}
