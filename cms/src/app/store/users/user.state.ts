import User from "../../models/user.model";

export interface UsersState {
    users: User[];
    loading: boolean;
    error: string;
}

export const initializeUsersState = (): UsersState => ({
    loading: false,
    users: [],
    error: null,
});
