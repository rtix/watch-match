<template>
  <button class="button" :checked.attr="checked">
    <slot />
  </button>
</template>

<script lang="ts" setup>
defineProps<{ checked?: boolean }>();
</script>

<style scoped>
.button {
  background: none;
  border: none;
  font-size: inherit;
  display: inline-flex;
  color: inherit;
  align-items: center;
  justify-content: center;

  padding: 0.5em;
}

.button[checked="true"] {
  background-color: var(--color-accent);
  color: var(--background-color);
}

@media (hover: hover) and (pointer: fine) {
  .button:hover {
    cursor: pointer;
    background-color: var(--color-accent);
    color: var(--color-background);
  }
}

.button:active {
  cursor: pointer;
  background-color: var(--color-accent);
  color: var(--color-background);
}
</style>
