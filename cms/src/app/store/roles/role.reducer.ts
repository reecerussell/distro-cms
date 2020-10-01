import { initializeRoleState, RoleListState, RoleState } from "./role.state";
import * as RoleActions from "./role.action";
import Role from "src/app/models/role.model";

export type Action = RoleActions.All;

const defaultRoleStates: RoleState[] = [
    {
        ...(initializeRoleState() as RoleState),
    },
];

const defaultState: RoleListState = {
    roles: defaultRoleStates,
    loading: false,
    pending: 0,
};

const mergeRoles = (existingRoles: RoleState[], newRole: Role) =>
    existingRoles
        .filter((x) => x.id != newRole.id)
        .concat({
            ...initializeRoleState(),
            ...newRole,
        })
        .sort((a, b) => {
            if (a.name < b.name) {
                return -1;
            }
            if (a.name > b.name) {
                return 1;
            }
            return 0;
        });

export const RoleReducer = (state = defaultState, action: Action) => {
    switch (action.type) {
        case RoleActions.GET_ROLES:
            return { ...state, loaded: false, loading: true };

        case RoleActions.GET_ROLES_SUCCESS:
            return {
                ...state,
                roles: action.payload.map(
                    (role) =>
                        ({
                            ...initializeRoleState(),
                            ...role,
                        } as RoleState)
                ),
                loading: false,
                loaded: true,
                error: null,
            };

        case RoleActions.GET_ROLES_ERROR:
            return {
                ...state,
                loaded: false,
                loading: false,
                error: action.error,
            };

        /*
            GET ROLE ACTIONS
        */

        case RoleActions.GET_ROLE:
            return { ...state, loaded: false, loading: true };

        case RoleActions.GET_ROLE_SUCCESS:
            return {
                ...state,
                roles: mergeRoles(state.roles, action.payload),
                loading: false,
                loaded: true,
                error: null,
            };

        case RoleActions.GET_ROLE_ERROR:
            return {
                ...state,
                loaded: false,
                loading: false,
                error: action.error,
            };

        /*
            CREATE ROLE ACTIONS
        */

        case RoleActions.CREATE_ROLE:
            return { ...state, loaded: false, loading: true };

        case RoleActions.CREATE_ROLE_SUCCESS:
            return {
                ...state,
                roles: mergeRoles(state.roles, action.payload),
                loading: false,
                loaded: true,
                error: null,
            };

        case RoleActions.CREATE_ROLE_ERROR:
            return {
                ...state,
                loaded: false,
                loading: false,
                error: action.error,
            };

        /*
            UPDATE ROLE ACTIONS
        */

        case RoleActions.UPDATE_ROLE:
            return { ...state, loaded: false, loading: true };

        case RoleActions.UPDATE_ROLE_SUCCESS:
            return {
                ...state,
                roles: mergeRoles(state.roles, action.payload),
                loading: false,
                loaded: true,
                error: null,
            };

        case RoleActions.UPDATE_ROLE_ERROR:
            return {
                ...state,
                roles: mergeRoles(state.roles, action.payload),
                loaded: false,
                loading: false,
                error: action.error,
            };

        /*
            DELETE ROLE ACTIONS
        */

        case RoleActions.DELETE_ROLE:
            return { ...state, loaded: false, loading: true };

        case RoleActions.DELETE_ROLE_SUCCESS:
            return {
                ...state,
                roles: state.roles.filter((x) => x.id != action.id),
                loading: false,
                loaded: true,
                error: null,
            };

        case RoleActions.DELETE_ROLE_ERROR:
            return {
                ...state,
                loaded: false,
                loading: false,
                error: action.error,
            };
    }
};
