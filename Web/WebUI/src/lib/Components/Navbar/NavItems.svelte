<script lang="ts">

    import {link} from 'svelte-spa-router'
    import Logo from "./Logo.svelte";

</script>
    
    <!-- <li class="nav-item item-0" on:click={() => console.log("Hello World")}>
        TODO change from icon to div with :after and :before elements for easier anims -->
        <!-- <Fa rotate={'360'} icon={faBars} color={'#FFFFFF'} size={'lg'} /> -->
    <!-- </li> -->
    <li class="nav-item item-1">
        <a href="/About" use:link>About Us</a>
    </li>
    <li class="nav-item item-2">
        <a href="/Contact" use:link>Contact</a>
    </li>
    
    <Logo />
    
    <li class="nav-item item-3">
        <a href="/Profile" use:link>Profile</a>
    </li>
    
    <style>
        
        li {
            flex: 0 1 auto;
            width: 10%;
            text-align: center;
            font-size: 1.7vw;
            cursor: pointer;
            margin: 0 0.5rem;
        }
        a {
            color: white;
            transition: 0.5s;
        }
        li:last-child > a{
            color: rgb(74, 74, 74);
        }
        li:last-child > a:hover {
            color: white;
        }
        a:hover {
            color: rgb(216, 211, 211);
        }
        .item-3 {
            margin-right: 0;
            margin-left: auto;
        } 
        .item-0 {
            margin-right: 2rem;
            margin-left: 1rem;
            width: auto;
        }
        .item-0:hover {
            color:rgb(178, 175, 175);
        }
    </style>
    