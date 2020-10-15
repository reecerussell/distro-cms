import { initializeUsersState } from "./user.state";
import * as UserActions from "./user.action";
import User from "src/app/models/user.model";

type Action = UserActions.All;

const defaultState = initializeUsersState();

const mergeUsers = (existsingUsers: User[], newUser: User): User[] => {
    const sortFunc = (a, b) => {
        if (a.firstname < b.firstname) {
            return -1;
        }
        if (a.firstname > b.firstname) {
            return 1;
        }
        return 0;
    };

    return existsingUsers
        .filter((x) => x.id !== newUser.id)
        .concat(newUser)
        .sort(sortFunc);
};

export const UserReducer = (state = defaultState, action: Action) => {
    switch (action.type) {
        case UserActions.GET_USERS:
            return { ...state, loading: true };
        case UserActions.GET_USERS_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                users: action.users,
            };
        case UserActions.GET_USERS_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case UserActions.GET_USER:
            return { ...state, loading: true };
        case UserActions.GET_USER_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                users: mergeUsers(state.users, action.user),
            };
        case UserActions.GET_USER_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case UserActions.UPDATE_USER:
            return { ...state, loading: true };
        case UserActions.UPDATE_USER_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                users: mergeUsers(state.users, action.user),
            };
        case UserActions.UPDATE_USER_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
    }
};
