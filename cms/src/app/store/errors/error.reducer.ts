import { initializeErrorState, ErrorState } from "./error.state";
import * as ErrorActions from "./error.action";

export type Action = ErrorActions.All;

const defaultState: ErrorState = {
    ...initializeErrorState(),
};

export const ErrorReducer = (state = defaultState, action: Action) => {
    console.log("Error Reducer", action);
    switch (action.type) {
        case ErrorActions.ADD_ERROR:
            console.log(state, action.error);
            return { ...state, errors: state.errors.concat(action.error) };

        case ErrorActions.CLEAR_ERRORS:
            return {
                ...state,
                errors: [],
            };
    }
};
