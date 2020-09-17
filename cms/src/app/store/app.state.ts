import { ItemListState } from "./dictionary/item.state";
import { RoleListState } from "./roles/role.state";

export default interface AppState {
    roles: RoleListState;
    dictionary: ItemListState;
}
