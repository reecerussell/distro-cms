export interface ErrorState {
    errors: string[];
}

export const initializeErrorState = () => ({
    errors: [],
});
