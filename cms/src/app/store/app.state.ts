import { RoleListState } from "./roles/role.state";
import { ErrorState } from "./errors/error.state";

export default interface AppState {
    roles: RoleListState;
    errors: ErrorState;
}
