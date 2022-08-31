<script lang="ts">

    import type User from "../../Models/user";
    import { agent } from "../../Utils/agent";
    import { link, push } from "svelte-spa-router";
    import FormField from "./FormField.svelte";
    import { jwtToken, userProfile } from "../../Stores/stores";
import Loader from "../Common/Loader.svelte";
import { get } from "svelte/store";
    
    export let fields = ['firstname', 'lastname', 'username','email', 'password'];
    export let type: 'Login' | 'Signup';
    
    let isOk:Array<boolean> = [];
    let canSubmit = false;
    let loading = false;

    $: {
        canSubmit = true;
        isOk.forEach(val => {
            if(!val) canSubmit=false;
        })
    }
    
    let serverError:string;
    
    function getFormData(form): User {
        const formData = new FormData(form);
        const data={};
        for(let field of formData){
            const [key, value] = field;
            data[key] = value;
        }
        return data as User;
    }
    
    async function onSubmit (e) {
        loading = true;
        setTimeout(async () => {

        const userData = getFormData(e.target);
        try{
            const [status, message] = type === "Signup" ? await agent.Account.SignUp(userData) : await agent.Account.LogIn(userData);

            if(status !== 200){
                serverError = await message as string;
                loading = canSubmit = false;
            } else {
                serverError = '';

                jwtToken.set(await message as string);
                localStorage.setItem("jwt", get(jwtToken))
                userProfile.set(userData);

                loading = canSubmit = false;
                push('/');
            }
            
        } catch(err) {
            loading = canSubmit = false;
            console.log(err);
        }
        }, 1000);
    }
    
    </script>
    <div class="all">
        <div class="container">
            <form class="form" on:submit|preventDefault={onSubmit}>
                <h1>{type==='Signup'?'Sign Up!':'Log In!'}</h1>
                <div class="fields">
                {#each fields as field, i}
                    {#if field === 'email'}
                        <FormField name={field} regex={new RegExp(/^\S+@\S+\.\S+$/)} bind:isOk={isOk[i]} />
                    {:else}
                        <FormField name={field} minLen={5} maxLen={12} bind:isOk={isOk[i]} />
                    {/if}
                {/each}
                </div>
                {#if serverError}
                    <span class="form-error">
                        <p>{serverError}</p>
                    </span>
                {/if}
                <button class="submit" disabled={!canSubmit || loading} class:disabled="{!canSubmit || loading}" type="submit">
                    {#if loading}
                        <Loader size={1} inElement={true} color='white'/>
                    {:else}
                        Submit
                    {/if}
                </button>
            </form>
            <div class="links">
                <a href={type === 'Signup'?"/Account/login":"/Account/signup"} use:link>{type === 'Signup' ? "You already have an account?" : "You don't have an account?"}</a>
                <a href="/Account/ForgotPassword" use:link>Forgot your password?</a>
            </div>
        </div>
    </div>
    
    <style>
    
        .links {
            margin-top: 2rem;
            display: flex;
            flex-direction: column;
        }
        h1 {
            font-size: 5rem;
        }
        form {
            border-radius: 1rem;
            width: 100%;
            height: auto;
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        form * {
            margin: 2rem;
        }
        .fields {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }
    
        .submit {
            font-family: 'Roboto Slab';
            border: none;
            border-radius: 2rem;
            width: 25rem;
            height: 5rem;
            cursor: pointer;
            font-size: 2.8rem;
            background-color: yellowgreen;
        }
        a {
            margin-top: auto;
            text-decoration: underline;
            color: blue;
        }
        .container {
            /* margin-bottom: 5rem; */
            font-family: 'Rubik';
            width: auto;
            height: auto;
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        .all {
            width: 100%;
            height: calc(100vh - 4.2vw);
            min-height: 58rem;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: white;
        }
        .disabled {
            opacity: 45%;
            cursor:auto;
        }
        .form-error{
            text-align: center;
        }
        .form-error p {
            font-family: 'Raleway';
            display: block;
            background-color: rgba(247, 138, 138, 0.2);
            padding: 0.5rem;
            color: red;
            margin: 0.5rem 1rem;
        }
    
    </style>