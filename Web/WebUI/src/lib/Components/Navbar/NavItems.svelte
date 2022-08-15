<script lang="ts">
    import Logo from "./Logo.svelte";
</script>
    
    <li class="nav-item item-0">
        <!-- svelte-ignore a11y-missing-attribute -->
        <!-- TODO add hamburger menu -->
        <a>Menu</a>
        <!-- <icon></icon> -->
    </li>
    <li class="nav-item item-1">
        <!-- svelte-ignore a11y-missing-attribute -->
        <a>About Us</a>
    </li>
    <!-- svelte-ignore a11y-missing-attribute -->
    <li class="nav-item item-2">
        <a>Contact</a>
    </li>
    
    <Logo />
    
    <li class="nav-item item-3">
        <!-- svelte-ignore a11y-missing-attribute -->
        <a>Profile</a>
    </li>
    
    <style>
        
        li {
            /* TODO: add fonts */
            flex: 0 1 auto;
            color: black;
            width: 10%;
            text-align: center;
            font-size: 1.8vw;
            cursor: pointer;
            margin: 0 20px;
        }
        a:hover {
            transition: 0.5s;
            color: rgb(231, 246, 98);
        }
        .item-3 {
            margin-right: 0;
            margin-left: auto;
        }
        .item-0 {
            margin-left: 0;
        }
    </style>
    