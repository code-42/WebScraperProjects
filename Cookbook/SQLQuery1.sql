SELECT * FROM Recipe

SELECT * FROM Ingredient

SELECT a.Name FROM Ingredient a
INNER JOIN RecipeIngredient b ON a.Id = b.IngredientId
WHERE b.RecipeId = 1;

UPDATE Recipe
SET Name = 'Salad Deluxe'
WHERE Id = 1;
