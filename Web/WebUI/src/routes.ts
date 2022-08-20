import Products from "./lib/Components/Homepage/Dashboard/Products.svelte";
import NotFound from "./lib/Components/Errors/NotFound.svelte";

export const routes: any = {
    '/': Products,
    '*': NotFound
}