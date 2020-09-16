import Role from "../../models/role.model";

export interface RoleState extends Role {
    loading: boolean;

    editable: boolean;
    edited: boolean;
    editing: boolean;

    selected: boolean;
    refreshing: boolean;

    create: boolean;
    error: boolean;
}

export const initializeRoleState = () => ({
    loading: false,

    editable: true,
    edited: false,
    editing: false,

    selected: false,
    refreshing: false,

    create: false,
    error: false,
});

export interface RoleListState {
    roles: RoleState[];
    loading: boolean;
    pending: number;
}

export const initializeRoleListState = () => ({
    loading: false,
    pending: 0,
});
