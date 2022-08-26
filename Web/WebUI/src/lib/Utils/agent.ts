import type Product from "../Models/product";

const apiUrl = "http://localhost:5000/api";

const postProducts = async (url: string, product: Product) => {
    let response;
    try{
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(product)
        });
    } catch (error) {
        console.log(error);
    }
    return response.json();
}

function getProducts<T>(url: string): Promise<T> {
    return fetch(url)
        .then(response => {
            if(!response.ok) {
                throw new Error(response.statusText);
            }
            return response.json().then(data => data as T);
        })
}

function getProduct<T>(url: string): Promise<T> {
    return fetch(url)
        .then(response => {
            if(!response.ok){
                throw new Error(response.statusText);
            }
            return response.json().then(data => data as T)
        })
}

export const agent = {
    Products: {
        getAll: async () => await getProducts<Array<Product>>(apiUrl+"/products"),
        getOne: async (id: string) => await getProduct<Product>(apiUrl+"/products/"+id),
        post: async (product: Product) => await postProducts(apiUrl+"/products", product)
    }
}