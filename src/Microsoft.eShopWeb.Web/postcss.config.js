const IN_PRODUCTION = process.env.NODE_ENV === 'production';

module.exports = {
  syntax: require('postcss-scss'),
  plugins: [
    require('@csstools/postcss-sass')(/* node-sass options */),
    require('autoprefixer'),

    IN_PRODUCTION &&
      require('@fullhuman/postcss-purgecss')({
        content: ['./**/*.Layout.fs', './**/*.Component.fs', './**/*.Page.fs'],
        defaultExtractor(content) {
          const contentWithoutStyleBlocks = content.replace(
            /<style[^]+?<\/style>/gi,
            ''
          );
          return (
            contentWithoutStyleBlocks.match(
              /[A-Za-z0-9-_/:]*[A-Za-z0-9-_/]+/g
            ) || []
          );
        },
      }),
    IN_PRODUCTION &&
      require('cssnano')({
        preset: 'default',
      }),
  ],
};
