<template>
  <header class="header">
    <div class="header__main">
      <nuxt-link to="/">
        <span class="header__title">Watch Match</span>
      </nuxt-link>
    </div>
    <div class="header__secondary">
      <div>
        <select v-model="$colorMode.preference">
          <option value="system">System</option>
          <option value="light">Light</option>
          <option value="dark">Dark</option>
        </select>
      </div>
    </div>
  </header>
</template>

<script lang="ts" setup></script>

<style>
.header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  padding-block: var(--spacing-sm);
  padding-inline: var(--spacing-md);
  background: linear-gradient(
    to top,
    hsla(var(--color-text--hsl), 0.2),
    #00000000
  );
}

.header__title {
  font-size: var(--text-xl);
  font-weight: bold;
}
</style>
