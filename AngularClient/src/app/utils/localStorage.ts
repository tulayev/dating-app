export const setItemToLocalStorage = (key: string, user: any) => {
    localStorage.setItem(key, JSON.stringify(user))
}

export const getItemFromLocalStorage = (key: string) => {
    return JSON.parse(localStorage.getItem(key))
}

export const removeItemFromLocalStorage = (key: string) => {
    localStorage.removeItem(key)
}
