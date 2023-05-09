from django.contrib import admin

# Register your models here.
from Tencu_Framework_Server.models.Version import Version
from Tencu_Framework_Server.models.asset_bundles.AssetBundles import AssetBundles
from Tencu_Framework_Server.models.asset_bundles.Dependency import Dependency

admin.register(AssetBundles)
admin.register(Version)
admin.register(Dependency)
