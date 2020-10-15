import { initializeUsersState } from "./User.state";
import * as UserActions from "./user.action";

type Action = UserActions.All;

const defaultState = initializeUsersState();

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
    }
};
